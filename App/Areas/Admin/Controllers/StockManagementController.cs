using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using iText.Kernel.Pdf;
using System.Text;
using iText.Html2pdf;
using ClosedXML.Excel;

namespace App.Areas.Admin.Controllers
{
    [Area ( "Admin" )]
    public class StockManagementController : Controller
    {
        private readonly ILogger<StockManagementController> _logger;
        private readonly ILogin login;
        private readonly IRegister register;
        private readonly IDropdownService _dropdownService;
        private readonly IStockManagement _stockManagement;
        StockManagementModel _model;
        List<ProductType> _productTypes;
        List<Unit> _unitTypes;
        public StockManagementController (
        ILogger<StockManagementController> _logger,
        ILogin login,
        IRegister register,
        IDropdownService _dropdownService,
        IStockManagement _stockManagement )
        {
            this.register = register;
            this._logger = _logger;
            this._dropdownService = _dropdownService;
            this.login = login;
            this._model = new StockManagementModel ( );
            this._stockManagement = _stockManagement;
            this._unitTypes = _dropdownService.GetUnits ( );
            this._productTypes = _dropdownService.GetProductTypes ( );
        }
        public IActionResult Index ( int currentPage = 1, string searchString = "", int PageSizeId = 10, string SortCol = "Id", string sortOrder = "Asc", string sortField = "Id", string? From = "current" )
        {
            _model.lstProductType = _productTypes;
            _model.lstUnit = _unitTypes;
            _model.ProductID = "PRD" + Guid.NewGuid ( ).ToString ( );
            int pageSize = 10;
            int userId = 0;
            int TotalCount = 0;
            ViewBag.FromPage = From;
            _model.lstAllStocks = _stockManagement.GetAll ( ref TotalCount, currentPage, searchString, pageSize, SortCol, sortOrder, userId );
            if ( From != "current" )
            {
                _model.ProductID = _model.lstAllStocks [0].ProductID;
                _model.ProductType = _model.lstAllStocks [0].ProductType;
                _model.Unit = _model.lstAllStocks [0].Unit;
                _model.Description = _model.lstAllStocks [0].Description;
                _model.Vendor = _model.lstAllStocks [0].Vendor;
                _model.StockLocation = _model.lstAllStocks [0].StockLocation;
                _model.StockQuantity = _model.lstAllStocks [0].StockQuantity;
                _model.UnitPrice = _model.lstAllStocks [0].UnitPrice;
                _model.TotalCost = _model.lstAllStocks [0].TotalCost;
            }
            return View ( _model );
        }
        public IActionResult Record ( )
        {
            List<Person> lstPerson = new List<Person> ( );
            lstPerson.Add ( new Person { Id = 0, Name = "Jacob", Location = "India" } );
            lstPerson.Add ( new Person { Id = 1, Name = "Jacob1", Location = "India1" } );
            lstPerson.Add ( new Person { Id = 2, Name = "Jacob2", Location = "India2" } );
            lstPerson.Add ( new Person { Id = 3, Name = "Jacob3", Location = "India3" } );
            return Json ( lstPerson );
        }
        public IActionResult Excel ( )
        {
            int pageSize = 100;
            int currentPage = 1; string searchString = ""; int PageSizeId = 10; string sortCol = "Id"; string sortOrder = "Asc"; string sortField = "Id";
            int TotalCount = 0;
            ViewBag.PageSizeId = PageSizeId;
            if ( string.IsNullOrEmpty ( sortField ) )
            {
                ViewBag.SortField = "ID";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if ( sortCol == sortField )
                {
                    ViewBag.SortOrder = sortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }
            ViewBag.searchString = searchString;

            _model.lstProductType = _productTypes;
            _model.lstUnit = _unitTypes;
            int userId = 0;
            List<StockManagementModel> stocks = _stockManagement.GetAll ( ref TotalCount, currentPage, searchString, pageSize, sortCol, sortOrder, userId );

            var resPageSizeDdl = _dropdownService.GetPageSizeDdl ( );

            if ( resPageSizeDdl [0].DbCode != -1 )
            {
                _model.lstPageSizeDdl = resPageSizeDdl;
            }


            int count = stocks.Count;
            _model.Pager = new JW.Pager ( TotalCount, currentPage, PageSizeId );

            _model.SearchString = searchString;
            _model.PageSizeId = PageSizeId;
            _model.lstAllStocks = stocks;

            DataTable dt = new DataTable ( "Grid" );
            dt.Columns.AddRange ( new DataColumn [] {
            new DataColumn("Product",typeof(string)),
            new DataColumn("Description",typeof(string)),
            new DataColumn("Vendor",typeof(string)),
            new DataColumn("Stock Location",typeof(string)),
             new DataColumn("Unit",typeof(string)),
            new DataColumn("TotalCost",typeof(double))} );
            #region Fill DataTable
            foreach ( var item in stocks )
            {
                var Product = from s in _productTypes.AsEnumerable ( ).Where ( x => x.id == Convert.ToInt32 ( item.ProductType ) )
                              select new
                              {
                                  ProductT = s.ProductTypeName,
                              };

                var UnitName = from s in _unitTypes.AsEnumerable ( ).Where ( x => x.id == Convert.ToInt32 ( item.Unit ) )
                               select new
                               {
                                   UnitName1 = s.UnitName,
                               };
                DataRow dr = dt.NewRow ( );
                dr ["Product"] = Product.First ( ).ProductT.ToString ( );
                dr ["Description"] = item.Description;
                dr ["Vendor"] = item.Vendor;
                dr ["Stock Location"] = item.StockLocation;
                dr ["Unit"] = UnitName.First ( ).UnitName1.ToString ( );
                dr ["TotalCost"] = Convert.ToDouble ( item.TotalCost );

                dt.Rows.Add ( dr );
                dt.AcceptChanges ( );
            }
            #endregion End DataFill

            using ( XLWorkbook wb = new XLWorkbook ( ) )
            {
                wb.Worksheets.Add ( dt );
                using ( MemoryStream stream = new MemoryStream ( ) )
                {

                    wb.SaveAs ( stream );
                    return File ( stream.ToArray ( ), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx" );
                }

            }
        }
        public IActionResult PrintRecord ( )
        {
            string wwwRootPath = Path.Combine ( Directory.GetCurrentDirectory ( ), "wwwroot" );
            string pdfPath = Path.Combine ( wwwRootPath, "PDF", "makepdf.pdf" ).ToString ( );

            int pageSize = 100;
            int currentPage = 1; string searchString = ""; string SortCol = "Id"; string sortOrder = "Asc";
            int TotalCount = 0;
            var result = _stockManagement.GetAll ( ref TotalCount, currentPage, searchString, pageSize, SortCol, sortOrder, 0 );
            StringBuilder stringBuilder = new StringBuilder ( );
            stringBuilder.Append ( @"
<style>  table {
font-family: Arial, Helvetica, sans-serif;
font-size:12px;
border-collapse: collapse;
width: 100%;
}table td, table th {
border: 1px solid #ddd;
padding: 5px;
}
/*table tr:nth-child(even){background-color: #f2f2f2;}*/
table tr:hover {background-color: #ddd;}
table th {
padding-top: 12px;
padding-bottom: 12px;
text-align: left;
/*background-color: #04AA6D;*/
background-color: #f2f2f2;
color: black;
} </style>
<table class='table table-bordered' ><thead><tr><th>Sr.No</th><th>Type</th><th>Description</th><th>Vendor</th><th>Location</th><th>Unit</th><th>Total</th></tr></thead><tbody>" );
            int i = 1;
            foreach ( var item in result )
            {
                var Product = from s in _productTypes.AsEnumerable ( ).Where ( x => x.id == Convert.ToInt32 ( item.ProductType ) )
                              select new
                              {
                                  ProductT = s.ProductTypeName,
                              };

                var UnitName = from s in _unitTypes.AsEnumerable ( ).Where ( x => x.id == Convert.ToInt32 ( item.Unit ) )
                               select new
                               {
                                   UnitName1 = s.UnitName,
                               };

                stringBuilder.Append ( $"<tr><td>{i++}</td><td>{Product.First ( ).ProductT}</td><td>{item.Description.ToString ( ).Substring ( 0, Math.Min ( item.Description.Length, 20 ) )}{( item.Description.Length > 20 ? "..." : "" )}</td><td>{item.Vendor}</td><td>{item.StockLocation}</td><td>{UnitName.First ( ).UnitName1}</td><td>{item.TotalCost}</td></tr>" );
                //Console.WriteLine ( "Value is: " + Math.Min ( item.Description.Length, 20 ) );
                _logger.LogInformation ( "Value is: " + Math.Min ( item.Description.Length, 20 ) );
            }
            stringBuilder.Append ( @"</tbody></table>" );

            using ( var document = HtmlConverter.ConvertToDocument ( stringBuilder.ToString ( ), new PdfWriter ( pdfPath ) ) )
            {

            }
            ViewBag.PdfPath = "makepdf.pdf";
            return View ( );
        }
        public IActionResult DeleteFunction ( string? From = "", string? ProductID = "0" )
        {
            RouteData.Values ["action"] = "StockList";
            if ( ProductID != "0" )
            {
                _model.ProductID = ProductID;
                var result = _stockManagement.Delete ( _model );
            }
            int currentPage = 1; string searchString = ""; int PageSizeId = 10; string sortCol = "Id"; string sortOrder = "Asc"; string sortField = "Id";
            ViewBag.PageSizeId = PageSizeId;
            if ( string.IsNullOrEmpty ( sortField ) )
            {
                ViewBag.SortField = "ID";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if ( sortCol == sortField )
                {
                    ViewBag.SortOrder = sortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }
            ViewBag.searchString = searchString;

            _model.lstProductType = _productTypes;
            _model.lstUnit = _unitTypes;
            //_model.ProductID = "PRD" + Guid.NewGuid ( ).ToString ( );
            int pageSize = 10;
            int userId = 0;
            int TotalCount = 0;
            List<StockManagementModel> stocks = _stockManagement.GetAll ( ref TotalCount, currentPage, searchString, pageSize, sortCol, sortOrder, userId );
            var resPageSizeDdl = _dropdownService.GetPageSizeDdl ( );

            if ( resPageSizeDdl [0].DbCode != -1 )
            {
                _model.lstPageSizeDdl = resPageSizeDdl;
            }


            int count = stocks.Count;
            _model.Pager = new JW.Pager ( TotalCount, currentPage, PageSizeId );

            _model.SearchString = searchString;
            _model.PageSizeId = PageSizeId;
            _model.lstAllStocks = stocks;

            return View ( "StockList", _model );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertStock ( StockManagementModel model )
        {

            if ( ModelState.IsValid )
            {
                model.CreatedBy = HttpContext.Session.GetString ( "UserName" );
                _stockManagement.Insert ( model );
            }
            _model = model;
            _model.lstProductType = _productTypes;
            _model.lstUnit = _unitTypes;
            _model.ProductID = "PRD" + Guid.NewGuid ( ).ToString ( );

            return View ( "Index", _model );

        }
        public IActionResult StockList ( int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortCol = "Id", string sortOrder = "Asc", string sortField = "Id" )
        {
            ViewBag.PageSizeId = PageSizeId;
            if ( string.IsNullOrEmpty ( sortField ) )
            {
                ViewBag.SortField = "ID";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if ( sortCol == sortField )
                {
                    ViewBag.SortOrder = sortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }
            ViewBag.searchString = searchString;

            _model.lstProductType = _productTypes;
            _model.lstUnit = _unitTypes;
            //_model.ProductID = "PRD" + Guid.NewGuid ( ).ToString ( );
            int pageSize = 10;
            int userId = 0;
            int TotalCount = 0;
            List<StockManagementModel> stocks = _stockManagement.GetAll ( ref TotalCount, currentPage, searchString, pageSize, sortCol, sortOrder, userId );

            var resPageSizeDdl = _dropdownService.GetPageSizeDdl ( );

            if ( resPageSizeDdl [0].DbCode != -1 )
            {
                _model.lstPageSizeDdl = resPageSizeDdl;
            }


            int count = stocks.Count;
            _model.Pager = new JW.Pager ( TotalCount, currentPage, PageSizeId );

            _model.SearchString = searchString;
            _model.PageSizeId = PageSizeId;
            _model.lstAllStocks = stocks;
            return View ( _model );
        }
    }
}
class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public Person ( )
    {
        this.Id = Id;
        this.Name = Name;
        this.Location = Location;
    }
}