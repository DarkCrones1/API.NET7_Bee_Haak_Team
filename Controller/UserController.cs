using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Controller.Base;
using Web_API_Bee_Haak.Data;
using Web_API_Bee_Haak.DTOS;
using AutoMapper;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

namespace Web_API_Bee_Haak.Controller;
[ApiController]
[Route("API_Bee_Haak/Account")]
// [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class UserController :ControllerBase
{
    private readonly IdentityUser user;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;
    private readonly SignInManager<IdentityUser> signInManager;

    public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.signInManager = signInManager;
    }

    

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<RequestAuthentication>> Register(UserCredencial userCredencial)
    {
        var user = new IdentityUser{Email = userCredencial.Email, UserName = userCredencial.Email};
        var result = await userManager.CreateAsync(user, userCredencial.PassWord);

        if (result.Succeeded)
        {
            return await BuildToken(userCredencial);
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<RequestAuthentication>> Login(UserCredencial userCredencial){
        var result = await signInManager.PasswordSignInAsync(userCredencial.Email, userCredencial.PassWord, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return await BuildToken(userCredencial);
        }
        else
        {
            return BadRequest("Incorrected Login");
        }
    }

    [HttpGet("Tokenrenovation")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="Admin" )]
    public async Task<ActionResult<RequestAuthentication>> Renove(){
        var emailclaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailclaim.Value;
        var userCredencial = new UserCredencial(){
            Email = email
        };

        return await BuildToken(userCredencial);
    }

    private async Task<RequestAuthentication> BuildToken(UserCredencial userCredencial){
        var claims = new List<Claim>(){
            new Claim("email", userCredencial.Email),
            new Claim("Rol", "User"),
        };

        var user = await userManager.FindByEmailAsync(userCredencial.Email);
        var claimsDB = await userManager.GetClaimsAsync(user);

        claims.AddRange(claimsDB);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["KeyJwt"]));
        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddYears(1);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

        return new RequestAuthentication()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiration
        };
    }

    [HttpPost("MakeAdmin")]
    public async Task<ActionResult> MakeAdmin(UserCredencial userCredencial){
        var user = await userManager.FindByEmailAsync(userCredencial.Email);
        await userManager.AddClaimAsync(user, new Claim("Rol","Admin"));
        await userManager.RemoveClaimAsync(user, new Claim("Rol","User"));
        return NoContent();
    }

    [HttpPost("RemoveAdmin")]
    public async Task<ActionResult> RemoveAdmin(UserCredencial userCredencial){
        var user = await userManager.FindByEmailAsync(userCredencial.Email);
        await userManager.AddClaimAsync(user, new Claim("Rol","User"));
        await userManager.RemoveClaimAsync(user, new Claim("Rol","Admin"));
        return NoContent();
    }


    // [HttpPost("MakeWorker")]
    // public async Task<ActionResult> MakeWorker(EmployeeCredencial employeeCredencial){
    //     var user = await userManager.FindByEmailAsync(employeeCredencial.Email);
    //     await userManager.AddClaimAsync(user, new Claim("Worker","2"));
    //     return NoContent();
    // }

    // [HttpPost("RemoveWorker")]
    // public async Task<ActionResult> RemoveWorker(EmployeeCredencial employeeCredencial){
    //     var user = await userManager.FindByEmailAsync(employeeCredencial.Email);
    //     await userManager.AddClaimAsync(user, new Claim("Worker","2"));
    //     return NoContent();
    // }
}