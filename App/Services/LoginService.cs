using App.Common;
using App.Interfaces;
using App.Models;
using Dapper;
using System.Data;

namespace App.Services
{
    public class LoginService : ILogin
    {
        private readonly IDapperService _dapper;
        public LoginService ( IDapperService dapper )
        {
            _dapper = dapper;
        }
        LoginModel ILogin.Get ( int id )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "@userId", id, DbType.Int32 );
            return _dapper.Get<LoginModel> ( CommonSp.getUserById, dbparams, commandType: CommandType.StoredProcedure );
        }

     

        LoginModel ILogin.Insert ( LoginModel userModel )
        {
            var dbparams = new DynamicParameters ( );
            //dbparams.Add("@userId", userModel.userId, DbType.Int32);
            dbparams.Add ( "@userName", userModel.Email, DbType.String );
            // dbparams.Add("")

            return _dapper.Insert<LoginModel> ( CommonSp.insertUserMaster, dbparams, commandType: CommandType.StoredProcedure );
        }

        LoginModel ILogin.Update ( LoginModel userModel )
        {
            throw new NotImplementedException ( );
        }

        LoginModel ILogin.Delete ( int Id )
        {
            throw new NotImplementedException ( );
        }

        LoginModel ILogin.IsUserValid ( LoginModel loginModel )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "Email", loginModel.Email, DbType.String );
            dbparams.Add ( "Password", loginModel.Password, DbType.String );

            var result = _dapper.Update<LoginModel> ( CommonSp.isValidUser, dbparams, commandType: CommandType.StoredProcedure );
            return result;
        }

        LoginModel ILogin.ProfileInsert ( LoginModel userModel )
        {
            throw new NotImplementedException ( );
        }

        LoginModel ILogin.ProfileById ( int userId )
        {
            throw new NotImplementedException ( );
        }

        List<LoginModel> ILogin.GetAllStates ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, int userId )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "@currentPage", currentPage, DbType.String );
            searchString = "%" + searchString + "%";
            dbparams.Add ( "@searchString", searchString, DbType.String );
            dbparams.Add ( "@pageSize", pageSize, DbType.String );

            sortOrder = sortCol + " " + sortOrder;

            dbparams.Add ( "@sortOrder", sortCol, DbType.String );
            dbparams.Add ( "@userId", userId, DbType.Int32 );
            dbparams.Add ( "TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output );
            var res = _dapper.GetAll<LoginModel> ( CommonSp.GetAllStates, dbparams, commandType: CommandType.StoredProcedure );

            TotalCount = dbparams.Get<int> ( "TotalCount" );
            return res;
        }

        List<LoginModel> ILogin.GetAll ( ref int TotalCount, int currentPage, string searchString, int pageSize, string sortCol, string sortOrder, string userId )
        {
            var dbparams = new DynamicParameters ( );
            dbparams.Add ( "@currentPage", currentPage, DbType.String );
            searchString = "%" + searchString + "%";
            dbparams.Add ( "@searchString", searchString, DbType.String );
            dbparams.Add ( "@pageSize", pageSize, DbType.String );

            sortOrder = sortCol + " " + sortOrder;

            dbparams.Add ( "@sortOrder", sortCol, DbType.String );
            dbparams.Add ( "@userId", userId, DbType.String );
            dbparams.Add ( "TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output );
            var res = _dapper.GetAll<LoginModel> ( CommonSp.GetAllUser, dbparams, commandType: CommandType.StoredProcedure );
            return res;
        }
    }
}

