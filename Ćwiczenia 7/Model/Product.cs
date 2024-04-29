namespace Ćwiczenia_7.Model;

public class Product
{
    public int IdProduct { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<ProductWarehouse> ProductWarehouses { get; set; }
}