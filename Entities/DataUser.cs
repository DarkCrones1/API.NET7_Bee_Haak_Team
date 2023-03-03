using Microsoft.AspNetCore.Identity;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.Entities;
public class DataUser: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string Addres {get;set;}
    public string RFC {get;set;} 
    public int UserId {get;set;}
    public DateTime BornDte {get;set;}
    public IdentityUser User {get;set;}
    public List<PaymentUser> PaymentClient {get;set;}
}