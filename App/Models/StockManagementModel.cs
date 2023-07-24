using JW;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static App.Models.DropdownModel;

namespace App.Models
{
    public class StockManagementModel
    {
        public int Id { get; set; }

        [Display ( Name = "Product Id" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? ProductID { get; set; }
        [Display ( Name = "Product Type" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? ProductType { get; set; }
        [Display ( Name = "Description" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? Description { get; set; }
        [Display ( Name = "Vendor" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? Vendor { get; set; }
        [Display ( Name = "Stock Location" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? StockLocation { get; set; }
        [Display ( Name = "Unit" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public String? Unit { get; set; }
        [Display ( Name = "Stock Quantity" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public double? StockQuantity { get; set; }
        [Display ( Name = "Unit Price" )]
        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public double? UnitPrice { get; set; }
        [Display ( Name = "Total Cost" )]

        [Required ( ErrorMessage = "This Fields Should not be Empty" )]
        public double? TotalCost { get; set; }
        public List<ProductType>? lstProductType { get; set; }
        public List<Unit>? lstUnit { get; set; }
        public String? CreatedBy { get; set; }

        public Pager? Pager { get; set; }
        public int PageSizeId { get; set; }
        public string? PageSizeValue { get; set; }
        public string? SearchString { get; set; }
        public List<PageSizeDdl>? lstPageSizeDdl { get; set; }

        public List<StockManagementModel>? lstAllStocks { get; set; }
        public string? DbCode { get; set; }
        public string? DbMsg { get; set; }
    }

    public class ProductType
    {
        public String? ProductTypeName { get; set; }
        public int id { get; set; }
    }
    public class Unit
    {
        public String? UnitName { get; set; }
        public int id { get; set; }

    }
}
