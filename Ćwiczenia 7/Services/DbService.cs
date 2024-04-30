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
    public async Task<Order?> GetOrderDetails(int id)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand("SELECT * FROM Order o JOIN Product_Warehouse w On o.IdOrder = w.IdOrder Where o.IdOrder=@1",connection);
        command.Parameters.AddWithValue("@1", id);
        var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }


        return new Order(){
            IdOrder = reader.GetInt32(0),
            IdProduct = reader.GetInt32(1),
            Amount = reader.GetInt32(2),
            CreatedAt = reader.GetDateTime(3),
            FullfiledAt = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
        };


    }

    public async Task<Product?> GetProductDetails(int id)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand("SELECT * FROM Product o JOIN Product_Warehouse w On o.IdProduct = w.IdProduct Where o.IdProduct=@1",connection);
        command.Parameters.AddWithValue("@1", id);
        var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }


        return new Product(){
            IdProduct = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Price = reader.GetDouble(3)
        };



    }

    public async Task<Warehouse?> GetWarehouseDetails(int id)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand("SELECT * FROM Warehouse o JOIN Product_Warehouse w On o.IdWarehouse = w.IdWarehouse Where o.IdWarehouse=@1",connection);
        command.Parameters.AddWithValue("@1", id);
        var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }


        return new Warehouse(){
            IdWarehouse = reader.GetInt32(0),
            Name = reader.GetString(1),
            Address = reader.GetString(2),
        };
    }
}