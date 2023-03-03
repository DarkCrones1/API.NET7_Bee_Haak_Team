namespace Web_API_Bee_Haak.DTOS;
public class ProductDTO
{
    public int Id {get;set;}
    public string Name {get;set;}
    public string Description {get;set;}
    public int Price {get;set;}
    public float Rating {get;set;}
    public int Quantity {get;set;}
    public bool Status {get;set;}
    public BrandDTO brand {get;set;}
    public CategoryDTO Category {get;set;}
}