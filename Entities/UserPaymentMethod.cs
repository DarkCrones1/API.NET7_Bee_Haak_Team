using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class UserPaymentMethod :BaseEntity
{
    public int PaymentMethodId {get;set;}
    public string UserId {get;set;}
    public PaymentMethod paymentMethod {get;set;}
    public IdentityUser User {get;set;}
}