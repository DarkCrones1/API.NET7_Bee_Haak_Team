using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.Controller.Base;
using Web_API_Bee_Haak.Data;
using Web_API_Bee_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Web_API_Bee_Haak.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web_API_Bee_Haak.Controller;
[ApiController]
[Route("API_Bee_Haak/UserData")]
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
    public async Task<ActionResult<UserDataDTO>> Get(){
        var user = await context.UserData.ToListAsync();

        return mapper.Map<UserDataDTO>(user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDataDTO>> Get(int id){
        var user = await context.UserData.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null){
            return NotFound();
        }

        return mapper.Map<UserDataDTO>(user);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        // userData.UserId = userId;
        context.Add(userData);
        await context.SaveChangesAsync();
        return Ok("Data User Added");
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