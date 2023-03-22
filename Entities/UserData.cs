using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class UserData: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    public string UserId {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
    public IdentityUser User {get;set;}
    public List<PaymentUser> PaymentClient {get;set;}
    public Cart Cart {get;set;}
}