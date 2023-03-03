using Microsoft.AspNetCore.Identity;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class ShoppingCart: BaseEntity
{
    public int DataUserId {get;set;}
    public DataUser DataUser {get;set;}
    public List<Order> Order {get;set;}
}