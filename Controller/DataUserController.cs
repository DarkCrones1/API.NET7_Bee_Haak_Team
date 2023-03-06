// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Web_API_Bee_Haak.Controller.Base;
// using Web_API_Bee_Haak.Data;
// using Web_API_Bee_Haak.DTOS;
// using AutoMapper;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Web_API_Bee_Haak.Entities;

// namespace Web_API_Bee_Haak.Controller;
// [ApiController]
// [Route("API_Bee_Haak/DataUser")]

// public class DataUserController: ControllerBase{
    
//     private readonly AplicationdbContext context;
//     private readonly IMapper mapper;

//     public DataUserController(AplicationdbContext context, IMapper mapper){
//         this.context = context;
//         this.mapper = mapper;
//     }

//     [HttpGet]
//     public async Task<ActionResult<UserDataDTO>> Get(){
//         var user = await context.DataUser.ToListAsync();

//         return mapper.Map<UserDataDTO>(user);
//     }

//     [HttpGet("{id:int}")]
//     public async Task<ActionResult<UserDataDTO>> Get(int id){
//         var user = await context.DataUser.FirstOrDefaultAsync(user => user.Id == id);

//         if (user == null){
//             return NotFound();
//         }

//         return mapper.Map<UserDataDTO>(user);
//     }

//     [HttpPost]
//     public async Task<ActionResult<UserDataCDTO>> Post([FromBody] UserDataCDTO userDataCDTO){
//         var exist = await context.DataUser.AnyAsync(dataUser => dataUser.RFC == userDataCDTO.RFC);

//         if (exist)
//         {
//             return BadRequest("RFC does exist");
//         }

//         var dataUser = mapper.Map<DataUser>(userDataCDTO);

//         dataUser.CreateOn = DateTime.Now;
//         dataUser.UpdateOn = DateTime.Now;
//         context.Add(dataUser);
//         await context.SaveChangesAsync();
//         return Ok("Data User Added");
//     }

//     [HttpPut("{id:int}")]
//     public async Task<ActionResult<DataUser>> UpdateById(int id, [FromBody]UserDataCDTO userDataCDTO){
//         var exist = await context.DataUser.AnyAsync();
//         if (!exist)
//         {
//             return BadRequest("User doesn't exist");
//         }

//         var user = mapper.Map<DataUser>(userDataCDTO);
//         user.Id = id;

//         user.UpdateOn = DateTime.Now;
//         context.Update(user);
//         await context.SaveChangesAsync();
//         return Ok("User Update");
//     }

//     [HttpDelete("{id:int}")]
//     public async Task<ActionResult<DataUser>> Delete(int id){
//         var user = await context.DataUser.FirstOrDefaultAsync(user => user.Id == id);
//         if (user == null)
//         {
//             return BadRequest("User dates Delete");
//         }

//         context.Remove(user);
//         await context.SaveChangesAsync();
//         return Ok("User Deleted");
//     }
// }