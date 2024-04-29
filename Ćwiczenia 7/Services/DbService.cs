using Ćwiczenia_7.Model;

namespace Ćwiczenia_7.Services;

public interface IDbService
{
    Task<Order?> GetOrderDetails(int id);
}
public class DbServiceL
{
    
}