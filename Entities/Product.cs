using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class Product: BaseEntity
{
    public string Name {get;set;}
    public string Description {get;set;} 
    public string Image {get;set;}
    public int Price {get;set;}
    public int Quantity {get;set;}
    public int CategoryId {get;set;}
    public int BrandId {get;set;}
    public int InventoryId {get;set;}
    public Category Category {get;set;}
    public Brand Brand {get;set;}
    public Inventory Inventory {get;set;}
}