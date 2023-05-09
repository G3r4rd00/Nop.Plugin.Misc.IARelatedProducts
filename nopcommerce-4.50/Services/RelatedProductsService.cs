

using DocumentFormat.OpenXml.Office2010.Excel;
using LinqToDB;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Misc.IARelatedProducts.Logic;
using Nop.Plugin.Misc.IARelatedProducts.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.IARelatedProducts.Services
{
	public class RelatedProductsService : IRelatedProductsService
	{
		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly IPictureService _pictureService;

		public IList<RelatedProductsDTO> Model { get; set; }
		

		public RelatedProductsService(IOrderService orderService, IProductService productService, IPictureService pictureService)
		{
			_orderService = orderService;
			_productService = productService;
			_pictureService = pictureService;
			Model = new List<RelatedProductsDTO>();
		}

		public async Task GenerateModel(double minSupport, double confidence)
		{
			var apriori = new Apriori();
			
			var orders =  await _orderService.SearchOrdersAsync();

			List<List<string>> dataset = orders.Select(o =>  _orderService.GetOrderItemsAsync(o.Id).Result.Select(oi =>  oi.ProductId.ToString()).ToList()).ToList();
			Model = new List<RelatedProductsDTO>();

			var result = apriori.GetFrequentItemSets(dataset, minSupport, confidence);
			if(result.Count > 1)
			{
				foreach (var item in result[1].GroupBy(r => r.Items[0], r => r))
				{
					var itemsO = item.OrderByDescending(r => r.Confidence).ToArray();
					int id = Convert.ToInt32(item.Key);
					var pic1 = await _pictureService.GetPicturesByProductIdAsync(id);
					var product = await _productService.GetProductByIdAsync(id);
					string url = (await _pictureService.GetPictureUrlAsync(pic1[0])).Url;
					RelatedProductsDTO e = new RelatedProductsDTO();
					e.MainProduct = new ProductDTO() { Id = id, Name = product.Name, Url = url };
					for (int x = 0; x < itemsO.Count(); x++)
					{
						id = Convert.ToInt32(itemsO[x].Items[1]);
						product = await _productService.GetProductByIdAsync(id);
						var pic2 = await _pictureService.GetPicturesByProductIdAsync(id);
						string url2 = (await _pictureService.GetPictureUrlAsync(pic2[0])).Url;
						var p = new ProductDTO() { Id = id, Name = product.Name, Url = url2, Support = itemsO[x].Support, Confidence = itemsO[x].Confidence };
						switch (x)
						{
							case 0: e.Related1 = p; break;
							case 1: e.Related2 = p; break;
							case 2: e.Related3 = p; break;
							case 3: e.Related4 = p; break;
							case 5: e.Related5 = p; break;
						}

					}
					Model.Add(e);
				}
			}
			

			return;
		}

		public async Task<IPagedList<RelatedProductsDTO>> SearchAsync(RelatedProductsSearchModel searchModel)
		{
			var records = await Model.ToListAsync();
			var paged = new PagedList<RelatedProductsDTO>(records, searchModel.Page - 1, searchModel.PageSize);

			return paged;
		}

		public async Task SaveModel()
		{
			foreach(var i in Model)
			{
				var related = await _productService.GetRelatedProductsByProductId1Async(i.MainProduct.Id);
				foreach (var p in related)
					await _productService.DeleteRelatedProductAsync(p);
				var ids = new ProductDTO[] { i.Related1, i.Related2, i.Related3, i.Related4, i.Related5 }.Where(r => r != null && r.Id > 0);
				int x = 0;
				foreach (var item in ids.OrderByDescending(r => r.Confidence))
				{
					await _productService.InsertRelatedProductAsync(new Core.Domain.Catalog.RelatedProduct()
					{
						DisplayOrder = x,
						ProductId1 = i.MainProduct.Id,
						ProductId2 = item.Id
					});
					x++;
				}
					
			}
		}
	}
}
