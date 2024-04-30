using System.Data;
using System.Data.SqlClient;
using Ćwiczenia_7.Model;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Ćwiczenia_7.Services;

public interface IDbService
{
    Task<Order?> GetOrderDetails(int id, int amount);
    Task<Product?> GetProductDetails(int id);
    Task<Warehouse?> GetWarehouseDetails(int id);
    void UpdateTable(DateTime dateTime);
    Task<ProductWarehouse?> GetMaxOrder();
    Task<ProductWarehouse?> GetByOrderId(int orderid);
    Task<ProductWarehouse> InsertWareHouese(int warehouseId, int productId, int orderId, int amount, DateTime createdAt);
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
    public async Task<Order?> GetOrderDetails(int id,int amount)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand("SELECT * FROM Order o JOIN Product_Warehouse w On o.IdOrder = w.IdOrder Where o.IdOrder=@1 AND w.Amount=@Amount",connection);
        command.Parameters.AddWithValue("@1", id);
        command.Parameters.AddWithValue("@Amount", amount);
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

    public async void UpdateTable(DateTime dateTime)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"UPDATE [ORDER] SET FulfilledAt = @date",
            connection
        );
        command.Parameters.AddWithValue("@date", dateTime);
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<ProductWarehouse?> GetMaxOrder()
    {
        await using var connection = await GetConnection();
        var sqlCommand =
            new SqlCommand(
                "SELECT * FROM Product_Warehouse WHERE IdProductWarehouse =(SELECT MAX(IdProductWarehouse) FROM Product_Warehouse)",
                connection);
        var reader = await sqlCommand.ExecuteReaderAsync();
        if (!reader.HasRows)
            return null;
        return new ProductWarehouse()
        {
            IdProductWarehouse = reader.GetInt32(0),
             IdProduct= reader.GetInt32(1),
            IdOrder = reader.GetInt32(2),
            IdWarehouse = reader.GetInt32(3),
            Amount = reader.GetInt32(4),
            Price = reader.GetDouble(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }
    
    public async Task<ProductWarehouse?> GetByOrderId(int order)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"SELECT * FROM Product_Warehouse WHERE IdOrder = @id",
            connection
        );

        command.Parameters.AddWithValue("@id", order);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new ProductWarehouse()
        {
            IdProductWarehouse = reader.GetInt32(0),
            IdProduct = reader.GetInt32(1),
            IdOrder = reader.GetInt32(2),
            IdWarehouse = reader.GetInt32(3),
            Amount = reader.GetInt32(4),
            Price = reader.GetDouble(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    public async Task<ProductWarehouse> InsertWareHouese(int warehouseId, int productId, int orderId, int amount, DateTime createdAt)
    {
        await using var connection = await GetConnection();
        var product = GetProductDetails(productId);
        var price = product.Result.Price * amount;
        var commandInsert = new SqlCommand("INSERT INTO Product_Warehouse " +
                                           "(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                                           "VALUES " +
                                           "(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);", 
            connection);
        commandInsert.Parameters.AddWithValue("IdWarehouse", warehouseId);
        commandInsert.Parameters.AddWithValue("IdProduct", productId);
        commandInsert.Parameters.AddWithValue("IdOrder", orderId);
        commandInsert.Parameters.AddWithValue("Amount", amount);
        commandInsert.Parameters.AddWithValue("Price", price);
        commandInsert.Parameters.AddWithValue("CreatedAt", createdAt);
        await commandInsert.ExecuteNonQueryAsync();
        return await GetMaxOrder();
    }
}