using App.Common;
using App.Interfaces;
using App.Models;
using Dapper;
using System.Data;

namespace App.Services
{
    public class RegisterService : IRegister
    {
        private readonly IDapperService _service;
        public RegisterService ( IDapperService _service )
        {
            this._service = _service;
        }

        List<RegisterModel> IRegister.GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string userId )
        {
            var dbparams = new DynamicParameters ( );
                dbparams.Add ( "@currentPage", currentPage, DbType.String );
           
            dbparams.Add ( "@searchString", searchString, DbType.String );
            dbparams.Add ( "@pageSize", pageSize, DbType.String );
            sortOrder = sortCol + " " + sortOrder;
            dbparams.Add ( "@sortOrder", sortOrder, DbType.String );
            dbparams.Add ( "@userId", userId, DbType.Int32 );
            dbparams.Add ( "TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output );
            return _service.GetAll<RegisterModel> ( CommonSp.GetAllRegister, dbparams, commandType: CommandType.StoredProcedure );
        }

        List<RegisterModel> IRegister.GetRegisterModel ( RegisterModel registerModel )
        {
            var dbparams = new DynamicParameters ( );

            return _service.GetAll<RegisterModel> ( CommonSp.GetAllRegister, dbparams, commandType: CommandType.StoredProcedure );
        }
        RegisterModel IRegister.Insert ( RegisterModel registerModel )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add("@Id",registerModel.Id,DbType.String);
            dbparams.Add ( "@Email", registerModel.Email, DbType.String );
            dbparams.Add ( "@Password", registerModel.Password, DbType.String );
            dbparams.Add ( "@FirstName", registerModel.FirstName, DbType.String );
            dbparams.Add ( "@LastName", registerModel.LastName, DbType.String );
            dbparams.Add( "@CreatedBy",registerModel.CreatedBy, DbType.String );
            return _service.Update<RegisterModel> ( CommonSp.RegisterUser, dbparams, commandType: CommandType.StoredProcedure );
        }
    }
}
