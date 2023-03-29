using Web_API_Kaab_Haak.Validations;

namespace Web_API_Kaab_Haak.DTOS;
public class ProductCDTO
{
    public string Name {get;set;}
    public string Description {get;set;}
    [ArchiveValidationWeight(MaxWeightMB:4)]
    [TypeArchiveValidation(groupTypeArchive: GroupTypeArchive.Image)]
    public IFormFile Image {get;set;}
    public int Price {get;set;}
    public int Quantity {get;set;}
    public int CategoryId {get;set;}
    public int BrandId {get;set;}
    public int InventoryId {get;set;}
}
