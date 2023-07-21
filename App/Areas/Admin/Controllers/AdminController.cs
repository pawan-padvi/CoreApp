using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin
{
    [Area ( "Admin" )]
    public class AdminController : Controller
    {
        
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogin login;
        private readonly IRegister register;
        private readonly IDropdownService _dropdownService;
        RegisterModel registerModel = new RegisterModel ( );
       public AdminController ( ILogin login, IHttpContextAccessor contextAccessor, IRegister register, IDropdownService dropdownService )
        {
            this._contextAccessor = contextAccessor;
            this.register = register;
            this.login = login;
            _dropdownService = dropdownService;
        }

        public IActionResult Index ( int currentPage = 1, string searchString = "", int PageSizeId = 10, string sortCol = "FirstName", string sortOrder = "DESC", string sortField = "FirstName" )
        {
            ViewBag.PageSizeId = PageSizeId;
            if ( string.IsNullOrEmpty ( sortField ) )
            {
                ViewBag.SortField = "FirstName";
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
            RegisterModel model = new RegisterModel ( );
            //var result = register.GetRegisterModel ( model );
           
            int TotalCount = 0;
            String? UserId = "0"; //HttpContext.Session.GetString ( "UserName" ) ;
            var res = register.GetAll ( ref TotalCount, currentPage, searchString, PageSizeId, sortField, ViewBag.SortOrder, UserId );
            var resPageSizeDdl = _dropdownService.GetPageSizeDdl ( );

            if ( resPageSizeDdl [0].DbCode != -1 )
            {
                model.lstPageSizeDdl = resPageSizeDdl;
           }

            model.ListRegister = res;
            int count = res.Count;
            model.Pager = new JW.Pager ( count, currentPage, PageSizeId );

            model.SearchString = searchString;
            model.PageSizeId = PageSizeId;

            return View ( model );
        }
        public IActionResult Profile ( String Email ) { 

            return View();
        }
    }
}
