using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class UserAddress :BaseEntity
{
    public string StreetAddres {get;set;}
    public string ExteriorNumber {get;set;}
    public string InteriorNumber {get;set;}
    [StringLength(5)]
    public string PostalCode {get;set;}
    public string Town {get;set;}
    public string City {get;set;}
    public string UserId {get;set;}
    public IdentityUser User {get;set;}
}