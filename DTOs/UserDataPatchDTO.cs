using System.ComponentModel.DataAnnotations;

namespace Web_API_Kaab_Haak.DTOS;
public class UserDataPatchDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    public string Image {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}