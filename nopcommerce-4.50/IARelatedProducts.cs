using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Nop.Plugin.Misc.IARelatedProducts
{  
    public class IARelatedProducts : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;

        public bool HideInWidgetList => true;

        public IARelatedProducts(  IWebHelper webHelper, WidgetSettings widgetSettings, ISettingService settingService)
        {
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/IARelatedProducts/Configure";
        }

        public override async Task InstallAsync()
        {
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(IARelatedProductsDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(IARelatedProductsDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }

		public string GetWidgetViewComponentName(string widgetZone)
		{
			throw new NotImplementedException();
		}
	}
}