

using System.Data;
using Dapper;

namespace EasyGroceries.Order.Infrastructure.Contracts
{
    public interface IDapper : IDisposable
    {
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, T data, CommandType commandType = CommandType.StoredProcedure);
        int Execute<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, T data, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int InsertList<T>(string sp, T data, CommandType commandType = CommandType.StoredProcedure);
    }

}