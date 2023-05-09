
using Nop.Core;
using Nop.Plugin.Misc.IARelatedProducts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.IARelatedProducts.Services
{
    public interface IRelatedProductsService
    {
		

		IList<RelatedProductsDTO> Model { get; set; }

		Task<IPagedList<RelatedProductsDTO>> SearchAsync(RelatedProductsSearchModel searchModel);

		Task GenerateModel(double minSupport, double Confidence);

		Task SaveModel();
	}
}