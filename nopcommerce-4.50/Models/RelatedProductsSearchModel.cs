
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.IARelatedProducts.Models
{
    public record RelatedProductsSearchModel : BaseSearchModel
    {
        //public string Name { get; set; }
    
        public ColumnOptions[] Columns { get; set; }
        public Order[] Order { get; set; }
    }

    public class ColumnOptions
    {
        public string Data { get; set; }
        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

}
