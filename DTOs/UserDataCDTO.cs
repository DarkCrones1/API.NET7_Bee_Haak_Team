using System.ComponentModel.DataAnnotations;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.DTOS;
public class UserDataCDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CellPhoneNumber {get;set;}
    public IFormFile Image {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}