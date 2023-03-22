using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.DTOS;
public class UserDataDTO
{
    // public int Id {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    // public string UserId {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}