using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Entities.Base;

namespace Web_API_Bee_Haak.DTOS;
public class UserDataDTO: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string CPNumber {get;set;}
    public string Addres {get;set;}
    public string RFC {get;set;}
    public DateTime BornDte {get;set;}
}