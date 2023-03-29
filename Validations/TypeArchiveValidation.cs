using System.ComponentModel.DataAnnotations;

namespace Web_API_Kaab_Haak.Validations;
public class TypeArchiveValidation: ValidationAttribute
{
    private readonly string[] validTypes;

    public TypeArchiveValidation(string[] validTypes)
    {
        this.validTypes = validTypes;
    }

    public TypeArchiveValidation(GroupTypeArchive groupTypeArchive)
    {
        if (groupTypeArchive == GroupTypeArchive.Image)
        {
            validTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null){
            return ValidationResult.Success;
        }

        IFormFile formFile = value as IFormFile;

        if (formFile == null){
            return ValidationResult.Success;
        }

        if (!validTypes.Contains(formFile.ContentType)){
            return new ValidationResult($"only files can be uploaded: {string.Join("," , validTypes)}");
        }

        return ValidationResult.Success;
    }
}