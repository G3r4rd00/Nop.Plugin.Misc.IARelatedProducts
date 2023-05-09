
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.IARelatedProducts.Models
{
    /// <summary>
    /// Represents a tax transaction log list model
    /// </summary>
    public record RelatedProductsListModel : BasePagedListModel<RelatedProductsDTO>    
    {
    }
}