using Microsoft.AspNetCore.Identity;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class UserData: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    public int UserId {get;set;}
    public DateTime BornDate {get;set;}
    public IdentityUser identityUser {get;set;}
    public List<PaymentUser> PaymentClient {get;set;}
    public Cart Cart {get;set;}
}