namespace Ćwiczenia_7.Model;

public class Order
{
    public int IdOrder { get; set; }
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FullfiledAt { get; set; }
    public List<Product> Products { get; set; }
    public List<ProductWarehouse> ProductWarehouses { get; set; }
}