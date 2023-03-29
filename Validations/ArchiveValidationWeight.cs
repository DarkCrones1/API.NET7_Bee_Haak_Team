using System.ComponentModel.DataAnnotations;

namespace Web_API_Kaab_Haak.Validations;
public class ArchiveValidationWeight :ValidationAttribute
{
    private readonly int maxWeightMB;

    public ArchiveValidationWeight(int MaxWeightMB)
    {
        maxWeightMB = MaxWeightMB;
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

        if (formFile.Length > maxWeightMB * 1024 * 1024){
            return new ValidationResult($"The weight of the file must be less than {maxWeightMB} MB");
        }

        return ValidationResult.Success;
    }
}