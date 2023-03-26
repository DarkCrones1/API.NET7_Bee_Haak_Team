using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class Cart: BaseEntity
{
    // public string UsuarioId {get;set;}
    public string UserId {get;set;}
    public IdentityUser User {get;set;}
    public List<CartItem> Items {get;set;}
    public double TotalPrice {
        get{
            return Items.Sum(CartItem => CartItem.Price * CartItem.Quantity);
        }
    }
}