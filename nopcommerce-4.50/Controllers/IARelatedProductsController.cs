

using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Messages;
using Nop.Plugin.Misc.IARelatedProducts.Services;
using Nop.Core;
using System.Threading.Tasks;
using Nop.Plugin.Misc.IARelatedProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Catalog;
using iTextSharp.text.html.simpleparser;
using Nop.Services.Media;
using DocumentFormat.OpenXml.Wordprocessing;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.IARelatedProducts.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class IARelatedProductsController : BasePluginController
    {

        #region Fields
		private readonly INotificationService _notificationService;
		private readonly IRelatedProductsService _relatedProductsService;
		private readonly IStoreContext _storeContext;
		private readonly ISettingService _settingService;
		private readonly ILocalizationService _localizationService;
		private readonly IPictureService _pictureService;
		#endregion

		#region Ctor

		public IARelatedProductsController(INotificationService notificationService, IPictureService pictureService, IRelatedProductsService relatedProductsService, IStoreContext storeContext, ISettingService settingService, ILocalizationService localizationService)
        {
			_pictureService = pictureService;
            _notificationService = notificationService;
            _relatedProductsService = relatedProductsService;
			_storeContext = storeContext;
			_settingService = settingService;
			_localizationService = localizationService;
		}

		#endregion


		#region Methods

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		public async Task<IActionResult> SaveModel()
		{
			await _relatedProductsService.SaveModel();
			_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
			return RedirectToAction("Configure");
		}

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpPost]
		public async Task<IActionResult> CreateModel()
		{
			var settings = await _settingService.LoadSettingAsync<ConfigurationModel>();
			await _relatedProductsService.GenerateModel(settings.Support, settings.Confidence);
			return await Configure();
		}

		[AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure()
        {
			ConfigurationModel model = await _settingService.LoadSettingAsync<ConfigurationModel>();
			return View("~/Plugins/Misc.IARelatedProducts/Views/Configure.cshtml", model); 
        }

	

		[AuthorizeAdmin]
		[Area(AreaNames.Admin)]
		[HttpPost, ActionName("Search")]
		public async Task<IActionResult> Search(RelatedProductsSearchModel searchModel) 
		{
			var data = await _relatedProductsService.SearchAsync(searchModel);

			var model = new RelatedProductsListModel().PrepareToGrid(searchModel, data, () =>
			{
				return data;
			});

			return Json(model);
		}


		[AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost]
		//, ActionName("Configure")]
  //      [FormValueRequired("save")]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
				return RedirectToAction("Configure");

			var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            await _settingService.SaveSettingAsync(model, storeId);
			await _settingService.ClearCacheAsync();

			_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

			return await Configure();
		}

        #endregion
    }
}