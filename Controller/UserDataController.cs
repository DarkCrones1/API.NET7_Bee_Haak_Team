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
[Route("WebAPI_Kaab_Haak/UserData")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserDataController: ControllerBase{
    
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public UserDataController(AplicationdbContext context, IMapper mapper, UserManager<IdentityUser> userManager){
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpGet]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<UserDataDTO>> Get(){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var Data = await context.UserData.FirstOrDefaultAsync(UserData => UserData.UserId == userId);

        return mapper.Map<UserDataDTO>(Data);
    }

    [HttpPost]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<UserDataCDTO>> Post([FromBody] UserDataCDTO userDataCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var exist = await context.UserData.AnyAsync(user => user.CPNumber == userDataCDTO.CPNumber);

        if (exist)
        {
            return BadRequest("This number already exist");
        }

        var userData = mapper.Map<UserData>(userDataCDTO);

        
        userData.CreateOn = DateTime.Now;
        userData.UpdateOn = DateTime.Now;
        userData.UserId = userId;
        context.Add(userData);
        await context.SaveChangesAsync();
        return Ok("User Info Added");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserData>> UpdateById(int id, [FromBody]UserDataCDTO userDataCDTO){
        var exist = await context.UserData.AnyAsync();
        if (!exist)
        {
            return BadRequest("User doesn't exist");
        }

        var user = mapper.Map<UserData>(userDataCDTO);
        user.Id = id;

        user.UpdateOn = DateTime.Now;
        context.Update(user);
        await context.SaveChangesAsync();
        return Ok("User Update");
    }

    [HttpPatch]
    public async Task<ActionResult> Patch( JsonPatchDocument<UserDataPatchDTO> patchDocument){
        if(patchDocument == null){
            return BadRequest();
        }

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var UserDataDB = await context.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (UserDataDB == null){
            return NotFound("User not found");
        }

        var UserDataDTO = mapper.Map<UserDataPatchDTO>(UserDataDB);

        patchDocument.ApplyTo(UserDataDTO, ModelState);

        var isvalid = TryValidateModel(UserDataDTO);
        if (!isvalid){
            return BadRequest(ModelState);
        }

        mapper.Map(UserDataDTO, UserDataDB);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserData>> Delete(int id){
        var user = await context.UserData.FirstOrDefaultAsync(user => user.Id == id);
        if (user == null)
        {
            return BadRequest("User dates Delete");
        }

        context.Remove(user);
        await context.SaveChangesAsync();
        return Ok("User Deleted");
    }
}