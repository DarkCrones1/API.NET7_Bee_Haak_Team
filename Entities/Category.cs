using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class Category :BaseEntity
{
    public string Name {get;set;}
    public string Description {get;set;}
    public string ImageUrl {get;set;}
}