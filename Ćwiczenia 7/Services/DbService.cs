using System.Data;
using System.Data.SqlClient;
using Ćwiczenia_7.Model;

namespace Ćwiczenia_7.Services;

public interface IDbService
{
    Task<Order?> GetOrderDetails(int id);
    Task<Product?> GetProductDetails(int id);
    Task<Warehouse?> GetWarehouseDetails(int id);
}
public class DbService(IConfiguration configuration) : IDbService
{
    private async Task<SqlConnection> GetConnection()
    {
        var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        if (sqlConnection.State != ConnectionState.Open)
            await sqlConnection.OpenAsync();
        return sqlConnection;
    }
    public Task<Order?> GetOrderDetails(int id)
    {
        await using var connection = GetConnection();
        var command = new SqlCommand("")
    }

    public Task<Product?> GetProductDetails(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Warehouse?> GetWarehouseDetails(int id)
    {
        throw new NotImplementedException();
    }
}