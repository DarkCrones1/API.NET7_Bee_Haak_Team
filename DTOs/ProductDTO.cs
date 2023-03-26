namespace Web_API_Kaab_Haak.DTOS;
public class ProductDTO
{
    public int Id {get;set;}
    public string Name {get;set;}
    public string Description {get;set;}
    public string imageURL {get;set;}
    public int Price {get;set;}
    public int Quantity {get;set;}
    public BrandDTO brand {get;set;}
    public CategoryDTO Category {get;set;}
}