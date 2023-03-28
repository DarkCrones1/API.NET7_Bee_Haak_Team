using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.DTOS;
public class UserDataDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CellPhoneNumber {get;set;}
    public string Image {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}