using App.Models;

namespace App.Interfaces
{
    public interface IRegister
    {
        public RegisterModel Insert ( RegisterModel registerModel );
        public List<RegisterModel> GetRegisterModel ( RegisterModel registerModel );
        public List<RegisterModel> GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, String userId );
    }
}
