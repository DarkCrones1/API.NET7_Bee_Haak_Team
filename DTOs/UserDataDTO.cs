using Microsoft.AspNetCore.Identity;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.DTOS;
public class UserDataDTO
{
    public string Id {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    // public int UserId {get;set;}
    public DateTime BornDte {get;set;}
}