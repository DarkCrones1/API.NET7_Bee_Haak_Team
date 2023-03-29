using Web_API_Kaab_Haak.Validations;

namespace Web_API_Kaab_Haak.DTOS;
public class BrandCDTO
{
    public string Name {get;set;}
    [ArchiveValidationWeight(MaxWeightMB:4)]
    [TypeArchiveValidation(groupTypeArchive: GroupTypeArchive.Image)]
    public IFormFile Image {get;set;}
}