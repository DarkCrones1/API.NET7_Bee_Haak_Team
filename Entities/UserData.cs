using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.Entities;
public class UserData: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    [StringLength(10)]
    public string CellPhoneNumber {get;set;}
    public string Image {get;set;}
    public string UserId {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
    public IdentityUser User {get;set;}
}