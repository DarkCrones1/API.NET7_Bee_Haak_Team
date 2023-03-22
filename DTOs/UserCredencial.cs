namespace Web_API_Kaab_Haak.DTOS;
using System.ComponentModel.DataAnnotations;

public class UserCredencial
{
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public string PassWord {get;set;}
    
}