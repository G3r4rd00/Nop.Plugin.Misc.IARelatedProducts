using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.IARelatedProducts.Services;
using Nop.Services.Authentication;

namespace Nop.Plugin.Misc.IARelatedProducts.Infrastructure
{

    public class NopStartup : INopStartup
    {
        public int Order => 100;

        public void Configure(IApplicationBuilder application)
        {
            
        }

        
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRelatedProductsService, RelatedProductsService>();
		}
    }
}