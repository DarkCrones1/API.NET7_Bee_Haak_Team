namespace Web_API_Bee_Haak.DTOS;
public class ProductCDTO
{
    public string Name {get;set;}
    public string Description {get;set;}
    public int Price {get;set;}
    public float Rating {get;set;}
    public int Quantity {get;set;}
    public bool Status {get;set;}
    public int CategoryId {get;set;}
    public int BrandId {get;set;}
    public int InventoryId {get;set;}
}
