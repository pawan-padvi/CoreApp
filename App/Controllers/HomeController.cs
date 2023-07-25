using App.Interfaces;
using App.Models;
using App.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using IronOcr;
using System.Text;
using DocumentFormat.OpenXml.EMMA;

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
                LoadImages ( );
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
                    LoadImages ( );
                    return View ( "index" );
                }
                else
                {
                    loginModel.Email = HttpContext.Request.Cookies ["WebAppUser"];
                    loginModel.Password = HttpContext.Request.Cookies ["WebAppPassword"];
                    LoadImages ( );
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
                LoadImages ( );
                ModelState.AddModelError ( "Success", result.DbMsg == null ? "" : result.DbMsg );
                return View ( "Index" );
            }
            else
            {
                LoadImages ( );
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
        public IActionResult LoadImages ( )
        {

            var files = Directory.GetFiles ( Directory.GetCurrentDirectory ( ) + "\\wwwroot\\uploads" );
            List<String> lstImage = new List<String> ( );

            if ( files.Length > 0 )
            {
                foreach ( var file in files )
                {
                    var filename = $"uploads/{Path.GetFileName ( file )}";
                    lstImage.Add ( filename );
                }

            }
            ViewData ["Images"] = lstImage;

            string? filePath = "";
            string? uniqueFileName = "";
            string? uploads = "";
            if ( fileUploadModel.imageUpload != null )
            {
                uniqueFileName = GetUniqueFileName ( fileUploadModel.imageUpload.FileName );
                uploads = Path.Combine ( Directory.GetCurrentDirectory ( ), "wwwroot\\uploads" );
                filePath = Path.Combine ( uploads, uniqueFileName );
                fileUploadModel.ImagePath = filePath;

                ViewBag.FilePath = uniqueFileName;
                fileUploadModel.imageUpload.CopyTo ( new FileStream ( filePath, FileMode.Create ) );
                HttpContext.Session.SetString ( "Path", uniqueFileName );
            }

            //fileUploadModel.ImagePath = filePath;

            // IronOcrFunction ( filePath );
            return View ( "Index", fileUploadModel );

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
                var UploadFolder = Path.Combine ( Directory.GetCurrentDirectory ( ) + "\\wwwroot\\uploads" );
                if ( model.imageProfile != null )
                {
                    var FileName1 = Path.GetFileNameWithoutExtension ( model.imageProfile.FileName ) + "_" + Guid.NewGuid ( ).ToString ( ).Substring ( 0, 5 ) + Path.GetExtension ( model.imageProfile.FileName );
                    var FilePath1 = Path.Combine ( UploadFolder, FileName1 );
                    registerModel.Profile1Url = $"uploads/{FileName1}";
                    model.imageProfile.CopyTo ( new FileStream ( FilePath1, FileMode.Create ) );
                }
                if ( model.imageProfile2 != null )
                {
                    var FileName2 = Path.GetFileNameWithoutExtension ( model.imageProfile2.FileName ) + "_" + Guid.NewGuid ( ).ToString ( ).Substring ( 0, 5 ) + Path.GetExtension ( model.imageProfile2.FileName );
                    var FilePath2 = Path.Combine ( UploadFolder, FileName2 );
                    registerModel.Profile2Url = $"uploads/{FileName2}"; ;
                    model.imageProfile2.CopyTo ( new FileStream ( FilePath2, FileMode.Create ) );
                }

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
            string? filePath = "";
            string? uniqueFileName = "";
            string? uploads = "";
            try
            {
                if ( model.imageUpload != null )
                {
                    uniqueFileName = model.imageUpload.FileName;
                    uploads = Path.Combine ( Directory.GetCurrentDirectory ( ), "wwwroot\\uploads" );
                    filePath = Path.Combine ( uploads, uniqueFileName );
                    model.ImagePath = filePath;
                    ViewBag.FilePath = uniqueFileName;
                    model.imageUpload.CopyTo ( new FileStream ( filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite ) );

                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( ex );
                HttpContext.Session.SetString ( "Path", uniqueFileName );
                LoadImages ( );
                return View ( fileUploadModel );
            }

            HttpContext.Session.SetString ( "Path", uniqueFileName );
            LoadImages ( );
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