using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.IARelatedProducts.Models
{
	public class ProductDTO 
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Url { get; set; }

		public double Support { get; set; }

		public double Confidence { get; set; }
	}

    public record RelatedProductsDTO: BaseNopEntityModel
    {
        public string Name { get; set; }

		public ProductDTO MainProduct { get; set; }

		public ProductDTO Related1 { get; set; }
		public ProductDTO Related2 { get; set; }
		public ProductDTO Related3 { get; set; }
		public ProductDTO Related4 { get; set; }
		public ProductDTO Related5 { get; set; }
	}


}
