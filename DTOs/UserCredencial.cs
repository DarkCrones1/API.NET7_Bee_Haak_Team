namespace Web_API_Bee_Haak.DTOS;
using System.ComponentModel.DataAnnotations;

public class UserCredencial
{
    public string UserName {get;set;}
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public string PassWord {get;set;}
    
}