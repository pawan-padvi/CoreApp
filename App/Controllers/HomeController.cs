using App.Interfaces;
using App.Models;
using App.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public SessionManager sessionManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogin login;
        private readonly IRegister register;
        LoginModel loginModel = new LoginModel ( );
        RegisterModel registerModel = new RegisterModel ( );
        FileUploadModel fileUploadModel;
        public HomeController ( ILogger<HomeController> logger, IRegister register, ILogin login, IHttpContextAccessor _contextAccessor )
        {
            sessionManager = new SessionManager ( _contextAccessor );
            _logger = logger;
            this.register = register;
            this.login = login;
            this._contextAccessor = _contextAccessor;
            this.fileUploadModel = new FileUploadModel ( );
        }

        public IActionResult Index ( )
        {
            if ( HttpContext.Session.GetString ( "UserName" ) != null && HttpContext.Session.GetString ( "PassWord" ) != null )
            {
                return View ( );
            }
            else
            {
                loginModel.Email = HttpContext.Session.GetString ( "UserName" );
                loginModel.Password = HttpContext.Session.GetString ( "PassWord" );

                var result = login.IsUserValid ( loginModel );
                if ( result.DbCode == "1" )
                {
                    string? name = loginModel.Email == null ? loginModel.Email : loginModel.Email;
                    string? pass = result.Password == null ? loginModel.Password : result.Password;
                    var cookieOptions = new CookieOptions ( );
                    cookieOptions.Expires = DateTime.Now.AddDays ( 1 );
                    cookieOptions.Path = "/";
                    HttpContext.Response.Cookies.Append ( "WebAppUser", name, cookieOptions );
                    HttpContext.Response.Cookies.Append ( "WebAppPassword", pass, cookieOptions );
                    HttpContext.Session.SetString ( "UserName", name );
                    HttpContext.Session.SetString ( "PassWord", pass );
                    return View ( "index" );
                }
                else
                {
                    loginModel.Email = HttpContext.Request.Cookies ["WebAppUser"];
                    loginModel.Password = HttpContext.Request.Cookies ["WebAppPassword"];
                    return View ( "Login", loginModel );
                }

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginCredentials ( LoginModel model )
        {
            loginModel.Email = model.Email;
            loginModel.Password = model.Password;
            var result = login.IsUserValid ( loginModel );
            if ( result.DbCode == "1" )
            {
                String? name = result.Email == null ? model.Email : result.Email;
                String? pass = result.Password == null ? model.Password : result.Password;
                var cookieOptions = new CookieOptions ( );
                cookieOptions.Expires = DateTime.Now.AddDays ( 1 );
                cookieOptions.Path = "/";
                HttpContext.Response.Cookies.Append ( "WebAppUser", name, cookieOptions );
                HttpContext.Response.Cookies.Append ( "WebAppPassword", pass, cookieOptions );
                HttpContext.Session.SetString ( "UserName", name );
                HttpContext.Session.SetString ( "PassWord", pass );

                ModelState.AddModelError ( "Success", result.DbMsg == null ? "" : result.DbMsg );
                return View ( "Index" );
            }
            else
            {
                ModelState.AddModelError ( "Error", result.DbMsg == null ? "" : result.DbMsg );
                return View ( "Login" );
            }

        }
        public IActionResult Logout ( )
        {
            //HttpContext.Response.Cookies.Delete ( "WebAppUser" );
            //HttpContext.Response.Cookies.Delete ( "WebAppPassword" );
            HttpContext.Session.Clear ( );
            loginModel = new LoginModel ( );
            loginModel.Email = HttpContext.Request.Cookies ["WebAppUser"];
            loginModel.Password = HttpContext.Request.Cookies ["WebAppPassword"];
            return View ( "Login" );
        }
        public IActionResult Privacy ( )
        {
            return View ( );
        }
        public IActionResult Register ( )
        {
            RegisterModel model = new RegisterModel ( );
            model.CreatedBy = HttpContext.Session.GetString ( "UserName" );
            return View ( model );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register ( RegisterModel model )
        {
            if ( ModelState is not null )
            {
                registerModel.Email = model.Email;
                registerModel.Password = model.Password;
                registerModel.FirstName = model.FirstName;
                registerModel.CreatedBy = HttpContext.Session.GetString ( "UserName" );
                Guid id = Guid.NewGuid ( );
                registerModel.Id = model.Id == null ? id.ToString ( ) : model.Id;
                registerModel.LastName = model.LastName;
                var result = register.Insert ( registerModel );
                if ( result.DbCode == "1" )
                {
                    ModelState.AddModelError ( "Success", result.DbMsg == null ? "" : result.DbMsg );
                    return View ( "Register" );
                }
                else
                {
                    return View ( "Register" );
                }
            }
            return View ( "Register" );
        }
        public IActionResult Login ( )
        {
            if ( HttpContext.Session.GetString ( "UserName" ) != null && HttpContext.Session.GetString ( "PassWord" ) != null )
            {
                return View ( "Index" );
            }
            loginModel = new LoginModel ( );
            loginModel.Email = HttpContext.Request.Cookies ["WebAppUser"];
            loginModel.Password = HttpContext.Request.Cookies ["WebAppPassword"];
            return View ( loginModel );
        }

        [ResponseCache ( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error ( )
        {
            return View ( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
        [HttpPost]
        public IActionResult Index ( FileUploadModel model )
        {
            string? filePath = null;
            if ( model.imageUpload != null )
            {
                var uniqueFileName = GetUniqueFileName ( model.imageUpload.FileName );
                var uploads = Path.Combine ( Directory.GetCurrentDirectory ( ), "wwwroot\\uploads" );
                filePath = Path.Combine ( uploads, uniqueFileName );
                model.ImagePath = filePath;

                ViewBag.FilePath = uniqueFileName;
                model.imageUpload.CopyTo ( new FileStream ( filePath, FileMode.Create ) );
            }
            fileUploadModel.ImagePath = filePath;
            return View ( fileUploadModel );
        }

        private string GetUniqueFileName ( string fileName )
        {
            fileName = Path.GetFileName ( fileName );
            return Path.GetFileNameWithoutExtension ( fileName )
                      + "_"
                      + Guid.NewGuid ( ).ToString ( ).Substring ( 0, 4 )
                      + Path.GetExtension ( fileName );
        }
    }
}