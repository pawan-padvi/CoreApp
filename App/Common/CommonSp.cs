using Microsoft.AspNetCore.Hosting;
namespace App.Common
{
    public class CommonSp
    {
        [Obsolete]
        private static Microsoft.AspNetCore.Hosting.IWebHostEnvironment? env;

        [Obsolete]
        public static void InitiateMembers ( Microsoft.AspNetCore.Hosting.IWebHostEnvironment enviroment )
        {
            env = enviroment;
        }
        public const string getAllPageSizeDdl = "usp_PageSizeMaster_GetAll";
        public const string getAllState = "GetAll_State";
        public const string getAllCity = "GetAll_City";
        public const string getAllGender = "GetAll_Gender";
        public const string getAllCourses = "GetAll_Courses";
        public const string GetValidUser = "isUserValidUser";
        public const string insertUserMaster = "InserUser";
        public const string GetAllUser = "GetAllUser";
        public const string GetAllStates = "GetAllStates";
        public const string getUserById = "GetUserById";
        public const string getCustomerById = "usp_UserMST_Customer_Get_By_Id";

        public const String RegisterUser = "SP_RegisterUser";
        public const String isValidUser = "ISVALIDUSER";
        public const String GetAllRegister = "sp_getAllRegister";
        public const String getProductType = "sp_ProductType";
        public const String getUnit = "sp_Unit";
        public const String InsertProductMaster = "SP_ProductMaster";
        public const String GellAllStocks = "SP_GetAllProduct";
        public const String DeleteProduct = "sp_DeleteProduct";


        [Obsolete]
        public static string ProfileImage => string.Format ( @"{0}", env.ContentRootPath );
    }
}
