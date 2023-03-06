using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.DTOS;
public class UserDataCDTO
{
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string StreetAddres {get;set;}
    public DateTime BornDte {get;set;}
}