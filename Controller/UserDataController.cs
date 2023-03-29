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
using Web_API_Kaab_Haak.Services;

namespace Web_API_Kaab_Haak.Controller;
[ApiController]
[Route("WebAPI_Kaab_Haak/UserData")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserDataController: ControllerBase{
    
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IStoreFiles storeFiles;
    private readonly string container = "UserImage";

    public UserDataController(AplicationdbContext context, IMapper mapper, UserManager<IdentityUser> userManager, IStoreFiles storeFiles){
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
        this.storeFiles = storeFiles;
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
    public async Task<ActionResult<UserDataCDTO>> Post([FromForm] UserDataCDTO userDataCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var exist = await context.UserData.AnyAsync(UserData => UserData.UserId == userId);

        if (exist)
        {
            return BadRequest("This User Data already exist");
        }

        var userData = mapper.Map<UserData>(userDataCDTO);

        if (userDataCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await userDataCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(userDataCDTO.Image.FileName);
                userData.Image = await storeFiles.SaveFile(content, extension, container, 
                    userDataCDTO.Image.ContentType);
            }
        }
        
        userData.CreateOn = DateTime.Now;
        userData.UpdateOn = DateTime.Now;
        userData.UserId = userId;
        context.Add(userData);
        await context.SaveChangesAsync();
        return Ok("User Data Added");
    }

    [HttpPut()]
    public async Task<ActionResult<UserData>> UpdateById([FromForm]UserDataCDTO userDataCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var userDB = await context.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
        if (userDB == null){
            return NotFound();
        }

        userDB = mapper.Map(userDataCDTO, userDB);

        if (userDataCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await userDataCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(userDataCDTO.Image.FileName);
                userDB.Image = await storeFiles.EditFile(content, extension, container, 
                    userDB.Image,
                    userDataCDTO.Image.ContentType);
            }
        }

        await context.SaveChangesAsync();
        return Ok("Data Update");
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

    [HttpDelete()]
    public async Task<ActionResult<UserData>> Delete(){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var Data = await context.UserData.FirstOrDefaultAsync(Data => Data.UserId == userId);
        if (Data == null)
        {
            return BadRequest("User Data doesn't exist");
        }

        context.Remove(user);
        await context.SaveChangesAsync();
        return Ok("Data Deleted");
    }
}