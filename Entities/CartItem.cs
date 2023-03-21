using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class CartItem :BaseEntity
{
    public int CartId {get;set;}
    public int ProductId {get;set;}
    public int Price {get;set;}
    public int Quantity {get;set;}
    public Cart Cart {get;set;}
    public Product Product {get;set;}
}