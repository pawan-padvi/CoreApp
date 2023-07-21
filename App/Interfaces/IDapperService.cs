using Dapper;
using System.Data.Common;
using System.Data;
using App.Common;

namespace App.Interfaces
{
    public interface IDapperService:IDisposable
    {
     
            DbConnection GetDbconnection ( );
           public T Get<T> ( string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure );
           public List<T> GetAll<T> ( string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure );
           public int Execute ( string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure );
           public T Insert<T> ( string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure );
           public T Update<T> ( string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure );

       
    }
}
