using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.Controller.Base;
using Web_API_Bee_Haak.Data;
using Web_API_Bee_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Web_API_Bee_Haak.Entities;

namespace Web_API_Bee_Haak.Controller;
[ApiController]
[Route("API_Bee_Haak/Category")]
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class CategoryController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;

    public CategoryController(AplicationdbContext context, IMapper mapper){
        this.context = context;
        this.mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<List<CategoryDTO>> Get(){
        var category = await context.Category.ToListAsync();
        return mapper.Map<List<CategoryDTO>>(category);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> Get(int id){
        var category = await context.Category.FirstOrDefaultAsync(category => category.Id == id);

        if (category == null){
            return NotFound();
        }

        return mapper.Map<CategoryDTO>(category);
    }

    [AllowAnonymous]
    [HttpGet("Name")]
    public IEnumerable<CategoryDTO> Get(String Name){
        var query = context.Category.Where(CategoryDTO => CategoryDTO.Name.Contains(Name)).ToList();

        return mapper.Map<IEnumerable<CategoryDTO>>(query);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post([FromBody] CategoryCDTO categoryCDTO){
        var exist = await context.Category.AnyAsync(category => category.Name == categoryCDTO.Name);
        if (exist)
        {
            return BadRequest("The name exist");
        }
        
        var category = mapper.Map<Category>(categoryCDTO);

        category.CreateOn = DateTime.Now;
        category.UpdateOn = DateTime.Now;
        context.Add(category);
        await context.SaveChangesAsync();
        return Ok("Category Created");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Category>> UpdateById(int id, [FromBody] CategoryCDTO categoryCDTO){
        var exist = await context.Category.AnyAsync(category => category.Id == id);
        if (!exist)
        {
            return BadRequest("Category doesn't exist");
        }

        var category = mapper.Map<Category>(categoryCDTO);
        category.Id = id;

        category.UpdateOn = DateTime.Now;
        context.Update(category);
        await context.SaveChangesAsync();
        return Ok("Category Update");
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Category>> PathById(int id, [FromBody] CategoryCDTO categoryCDTO){
        var exist = await context.Category.AnyAsync(category => category.Id == id);
        if (!exist)
        {
            return BadRequest("Category doesn't exist");
        }

        var category = mapper.Map<Category>(categoryCDTO);
        category.Id = id;
        category.UpdateOn = DateTime.Now;
        context.Update(category);
        await context.SaveChangesAsync();
        return Ok("Section Path");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Category>> Delete(int id){
        var category = await context.Category.FirstOrDefaultAsync(category => category.Id == id);
        if (category == null)
        {
            return BadRequest("Category doesn't exist");
        }

        context.Remove(category);
        await context.SaveChangesAsync();
        return Ok("Category Deleted");
    }
}