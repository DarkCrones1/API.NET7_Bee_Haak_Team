using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class Order
{
    public int ShoppingcartId {get;set;}
    public int ProductId {get;set;}
    public int Quantity {get;set;}
    public int Total {get;set;}
    public ShoppingCart Shoppingcart {get;set;}
    public Product Product {get;set;}
}