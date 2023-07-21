using App.Models;

namespace App.Interfaces
{
    public interface IStockManagement
    {
        public StockManagementModel Insert ( StockManagementModel stockManagementModel );
        public List<StockManagementModel> GetAllStockes ( StockManagementModel stockManagementModel );
        public List<StockManagementModel> GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, int userId );
        public StockManagementModel Delete ( StockManagementModel stockManagementModel );
    }
}
