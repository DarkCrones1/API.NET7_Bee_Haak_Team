using System.ComponentModel.DataAnnotations;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.DTOS;
public class UserDataCDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    // public int UserId {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}