using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Controller.Base;
using Web_API_Kaab_Haak.Data;
using Web_API_Kaab_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Web_API_Kaab_Haak.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Web_API_Kaab_Haak.Services;

namespace Web_API_Kaab_Haak.Controller;
[ApiController]
[Route("WebAPI_Kaab_Haak/Category")]
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class CategoryController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly IStoreFiles storeFiles;
    private readonly string container = "categoryimage";

    public CategoryController(AplicationdbContext context, IMapper mapper,IStoreFiles storeFiles ){
        this.context = context;
        this.mapper = mapper;
        this.storeFiles = storeFiles;
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
    public async Task<ActionResult<Category>> Post([FromForm] CategoryCDTO categoryCDTO){
        var exist = await context.Category.AnyAsync(category => category.Name == categoryCDTO.Name);
        if (exist)
        {
            return BadRequest("The name exist");
        }
        
        var category = mapper.Map<Category>(categoryCDTO);

        if (categoryCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await categoryCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(categoryCDTO.Image.FileName);
                category.Image = await storeFiles.SaveFile(content, extension, container,
                    categoryCDTO.Image.ContentType);
            }
        }

        category.CreateOn = DateTime.Now;
        category.UpdateOn = DateTime.Now;

        context.Add(category);
        await context.SaveChangesAsync();
        return Ok("Category Created");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Category>> UpdateById(int id, [FromForm] CategoryCDTO categoryCDTO){
        
        var categoryDB = await context.Category.FirstOrDefaultAsync(x => x.Id == id);
        if( categoryDB == null){
            return NotFound();
        }

        categoryDB = mapper.Map(categoryCDTO, categoryDB);

        if (categoryCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await categoryCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(categoryCDTO.Image.FileName);
                categoryDB.Image = await storeFiles.SaveFile(content, extension, container,
                    categoryCDTO.Image.ContentType);
            }
        }

        await context.SaveChangesAsync();
        return Ok("Category Update");
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoryPatchDTO> patchDocument){
        if(patchDocument == null){
            return BadRequest();
        }

        var CategoryDB = await context.Category.FirstOrDefaultAsync(x => x.Id == id);
        if(CategoryDB == null){
            return NotFound("Category not found");
        }

        var CategoryDTO = mapper.Map<CategoryPatchDTO>(CategoryDB);

        patchDocument.ApplyTo(CategoryDTO, ModelState);

        var isvalid = TryValidateModel(CategoryDTO);
        if (!isvalid){
            return BadRequest(ModelState);
        }

        mapper.Map(CategoryDTO, CategoryDB);

        await context.SaveChangesAsync();
        return NoContent();
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