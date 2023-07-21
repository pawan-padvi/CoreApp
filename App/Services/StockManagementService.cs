using App.Common;
using App.Interfaces;
using App.Models;
using Dapper;
using System.Data;

namespace App.Services
{
    public class StockManagementService : IStockManagement
    {
        private readonly IDapperService _service;
        public StockManagementService ( IDapperService _service )
        {
            this._service = _service;
        }

        StockManagementModel IStockManagement.Delete ( StockManagementModel stockManagementModel )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "@ProductID", stockManagementModel.ProductID, DbType.String );
            dbparams.Add ( "@CreatedBy", stockManagementModel.CreatedBy, DbType.String );
            return _service.Update<StockManagementModel> ( CommonSp.DeleteProduct, dbparams, commandType: CommandType.StoredProcedure ); ;
        }

        //List<StockManagementModel> IStockManagement.GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, int userId )
        //{
        //    throw new NotImplementedException ( );
        //}

        List<StockManagementModel> IStockManagement.GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, int userId )
        {
            var dbparams = new DynamicParameters ( );

            dbparams.Add ( "@currentPage", currentPage, DbType.String );
            searchString = "%" + searchString + "%";
            dbparams.Add ( "@searchString", searchString, DbType.String );
            dbparams.Add ( "@pageSize", pageSize, DbType.String );

            sortOrder = "PRD." + sortCol + " " + sortOrder;

            dbparams.Add ( "@sortOrder", sortOrder, DbType.String );
            dbparams.Add ( "@userId", userId, DbType.Int32 );
            dbparams.Add ( "TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output );

            var res = _service.GetAll<StockManagementModel> ( CommonSp.GellAllStocks, dbparams, commandType: CommandType.StoredProcedure );
            TotalCount = dbparams.Get<int> ( "TotalCount" );
            return res;
        }

        List<StockManagementModel> IStockManagement.GetAllStockes ( StockManagementModel stockManagementModel )
        {
            DynamicParameters dbparams = new DynamicParameters ( );
            return _service.GetAll<StockManagementModel> ( CommonSp.GellAllStocks, dbparams, commandType: CommandType.StoredProcedure );
        }

        StockManagementModel IStockManagement.Insert ( StockManagementModel stockManagementModel )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "@ProductID", stockManagementModel.ProductID, DbType.String );
            dbparams.Add ( "@Description", stockManagementModel.Description, DbType.String );
            dbparams.Add ( "@ProductType", stockManagementModel.ProductType, DbType.String );
            dbparams.Add ( "@Vendor", stockManagementModel.Vendor, DbType.String );
            dbparams.Add ( "@StockLocation", stockManagementModel.StockLocation, DbType.String );
            dbparams.Add ( "@Unit", stockManagementModel.Unit, DbType.String );
            dbparams.Add ( "@StockQuantity", stockManagementModel.StockQuantity, DbType.Double );
            dbparams.Add ( "@UnitPrice", stockManagementModel.UnitPrice, DbType.Double );
            dbparams.Add ( "@TotalCost", stockManagementModel.TotalCost, DbType.Double );
            dbparams.Add ( "@CreatedBy", stockManagementModel.CreatedBy, DbType.String );
            return _service.Update<StockManagementModel> ( CommonSp.InsertProductMaster, dbparams, commandType: CommandType.StoredProcedure );
        }
    }
}
