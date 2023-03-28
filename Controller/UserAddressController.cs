using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Controller.Base;
using Web_API_Kaab_Haak.Data;
using Web_API_Kaab_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Web_API_Kaab_Haak.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace Web_API_Kaab_Haak.Controller;
[ApiController]
[Route("WebAPI_Kaab_Haak/UserAddress")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserAddressController: ControllerBase{

    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public UserAddressController(AplicationdbContext context, IMapper mapper, UserManager<IdentityUser> userManager){
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<UserAddressDTO>> Get(){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var Address = await context.UserAddress.FirstOrDefaultAsync(UserAddress => UserAddress.UserId == userId);

        return mapper.Map<UserAddressDTO>(Address);
    }

    [HttpPost]
    public async Task<ActionResult<UserAddressCDTO>> Post([FromBody] UserAddressCDTO userAddressCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var exist = await context.UserAddress.AnyAsync(UserAddress => UserAddress.UserId == userId);

        if (exist)
        {
            return BadRequest("This User Address already exist");
        }

        var UserAddress = mapper.Map<UserAddress>(userAddressCDTO);

        UserAddress.CreateOn = DateTime.Now;
        UserAddress.UpdateOn = DateTime.Now;
        UserAddress.UserId = userId;
        context.Add(UserAddress);
        await context.SaveChangesAsync();
        return Ok("USer Address Added");
    }

    [HttpPut()]
    public async Task<ActionResult<UserAddress>> UpdateById([FromBody]UserAddressCDTO userAddressCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var exist = await context.UserData.AnyAsync();
        if (!exist)
        {
            return BadRequest("User doesn't exist");
        }

        var Address = mapper.Map<UserAddress>(userAddressCDTO);
        Address.UserId = userId;

        Address.UpdateOn = DateTime.Now;
        context.Update(Address);
        await context.SaveChangesAsync();
        return Ok("Address Update");
    }

    [HttpPatch]
    public async Task<ActionResult> Patch( JsonPatchDocument<UserAddressPatchDTO> patchDocument){
        if(patchDocument == null){
            return BadRequest();
        }

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var UserAddressDB = await context.UserAddress.FirstOrDefaultAsync(x => x.UserId == userId);
        if (UserAddressDB == null){
            return NotFound("User not found");
        }

        var UserAddressDTO = mapper.Map<UserAddressPatchDTO>(UserAddressDB);

        patchDocument.ApplyTo(UserAddressDTO, ModelState);

        var isvalid = TryValidateModel(UserAddressDTO);
        if (!isvalid){
            return BadRequest(ModelState);
        }

        mapper.Map(UserAddressDTO, UserAddressDB);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete()]
    public async Task<ActionResult<UserAddress>> Delete(){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var Adress = await context.UserAddress.FirstOrDefaultAsync(Adress => Adress.UserId == userId);
        if (Adress == null)
        {
            return BadRequest("User Address doesn't exist");
        }

        context.Remove(user);
        await context.SaveChangesAsync();
        return Ok("Address Deleted");
    }
}