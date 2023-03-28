using System.ComponentModel.DataAnnotations;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;

namespace Web_API_Kaab_Haak.DTOS;
public class UserAddressPatchDTO
{
    public string StreetAddres {get;set;}
    public string ExteriorNumber {get;set;}
    public string InteriorNumber {get;set;}
    [StringLength(5)]
    public string PostalCode {get;set;}
    public string Town {get;set;}
    public string City {get;set;}
}