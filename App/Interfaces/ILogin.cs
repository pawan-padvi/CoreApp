using App.Models;
namespace App.Interfaces
{
    public interface ILogin
    {
        public LoginModel Get ( int id );
        public List<LoginModel> GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, String userId );
        public LoginModel Insert ( LoginModel userModel );
        public LoginModel Update ( LoginModel userModel );
        public LoginModel Delete ( int Id );
        public LoginModel IsUserValid ( LoginModel userModel );
        public LoginModel ProfileInsert ( LoginModel userModel );
        public LoginModel ProfileById ( int userId );
      public List<LoginModel> GetAllStates ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, int userId );
    }
}
