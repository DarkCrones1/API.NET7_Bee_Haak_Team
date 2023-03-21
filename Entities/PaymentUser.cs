using Microsoft.AspNetCore.Identity;

namespace Web_API_Bee_Haak.Entities;
public class PaymentUser
{
    public int PaymentMethodId {get;set;}
    public int UserId {get;set;}
    public PaymentMethod PaymentMethod {get;set;}
    public UserData UserData {get;set;}
}