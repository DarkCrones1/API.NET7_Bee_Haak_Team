using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class Order :BaseEntity
{
    public string UserId {get;set;}
    public int CartId {get;set;}
    public IdentityUser identityUser {get;set;}
    public Cart cart {get;set;}
}