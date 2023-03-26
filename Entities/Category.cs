using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class Category :BaseEntity
{
    public string Name {get;set;}
    public string Description {get;set;}
    public string Image {get;set;}
}