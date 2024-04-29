

namespace Ćwiczenia_7.Model;

public class Warehouse
{
    public int IdWarehouse { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<ProductWarehouse> ProductWarehouses { get; set; }
}