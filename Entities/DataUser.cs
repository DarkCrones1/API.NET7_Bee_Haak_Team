using Microsoft.AspNetCore.Identity;

namespace Web_API_Bee_Haak.Entities;
public class DataUser: IdentityUser
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    public DateTime BornDte {get;set;}
    public DateTime CreteOn {get;set;}
    public DateTime UpdateOn {get;set;}
    public IdentityUser User {get;set;}
    public List<PaymentUser> PaymentClient {get;set;}
}