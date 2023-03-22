using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;
using Web_API_Kaab_Haak.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web_API_Kaab_Haak.DTOS;

namespace Web_API_Kaab_Haak.Controller;
[ApiController]
[Route("WebAPI_Kaab_Haak/Brand")] 
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class BrandController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;

    public BrandController(AplicationdbContext context, IMapper mapper){
        this.context = context;
        this.mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetBrands")]
    public async Task<ActionResult<List<BrandDTO>>> Get(){
        var brand = await context.Brand.ToListAsync();
        return mapper.Map<List<BrandDTO>>(brand);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetBrand")]
    public async Task<ActionResult<BrandDTO>> Get(int id){
        var brand = await context.Brand.FirstOrDefaultAsync(brand => brand.Id == id);

        if (brand == null){
            return NotFound();
        }

        return mapper.Map<BrandDTO>(brand);
    }

    [AllowAnonymous]
    [HttpGet("Name")]
    public IEnumerable<BrandDTO> Get(String Name){
        var query = context.Brand.Where(BrandDTO => BrandDTO.Name.Contains(Name)).ToList();

        return mapper.Map<IEnumerable<BrandDTO>>(query);
    }

    [HttpPost(Name = "Create-Brand")]
    public async Task<ActionResult<Brand>> Post([FromBody] BrandCDTO branCDTO){
        var exist = await context.Brand.AnyAsync(brand => brand.Name == branCDTO.Name);
        if (exist)
        {
            return BadRequest("Brand exist");
        }

        var brand = mapper.Map<Brand>(branCDTO);

        brand.CreateOn = DateTime.Now;
        brand.UpdateOn = DateTime.Now;
        context.Add(brand);
        await context.SaveChangesAsync();
        return Ok("Brand Created");
    }

    [HttpPut("{id:int}", Name = "UpdateBrand")]
    public async Task<ActionResult<Brand>> UpdateById(int id, [FromBody] BrandCDTO brandCDTO){
        var exist = await context.Brand.AnyAsync(brand => brand.Id == id);
        if (!exist)
        {
            return BadRequest("brand doesn't exist");
        }

        var brand = mapper.Map<Brand>(brandCDTO);
        brand.Id = id;

        brand.UpdateOn = DateTime.Now;
        context.Update(brand);
        await context.SaveChangesAsync();
        return Ok("Brand Updated");
    }

    [HttpDelete("{id:int}", Name = "DeleteBrand")]
    public async Task<ActionResult<Brand>> Delete(int id){
        var brand = await context.Brand.FirstOrDefaultAsync(brand => brand.Id == id);
        if (brand == null)
        {
            return BadRequest("Brand doesn't exist");
        }

        
        context.Remove(brand);
        await context.SaveChangesAsync();
        return Ok("Brand Deleted");
    }
}