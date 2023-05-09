
using Nop.Core.Configuration;
using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.IARelatedProducts.Models
{
    /// <summary>
    /// Represents plugin configuration model
    /// </summary>
    public record ConfigurationModel : ISettings
    {
        public ConfigurationModel()
        {
            
        }

        public int PageSize { get; set; } = 10;

		public double Support { get; set; }
		public double Confidence { get; set; }
	}
}