using System.ComponentModel.DataAnnotations;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;
using Web_API_Kaab_Haak.Validations;

namespace Web_API_Kaab_Haak.DTOS;
public class UserDataCDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    [StringLength(10)]
    public string CellPhoneNumber {get;set;}
    [ArchiveValidationWeight(MaxWeightMB:4)]
    [TypeArchiveValidation(groupTypeArchive: GroupTypeArchive.Image)]
    public IFormFile Image {get;set;}
    [DataType(DataType.DateTime)]
    public DateTime BornDate {get;set;}
}