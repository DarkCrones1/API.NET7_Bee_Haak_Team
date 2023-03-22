using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class Inventory: BaseEntity
{
    public List<Product> Stock {get;set;}
}