using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Entities.Base;
using Web_API_Kaab_Haak.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web_API_Kaab_Haak.DTOS;
using Microsoft.AspNetCore.JsonPatch;
using Web_API_Kaab_Haak.Services;

namespace Web_API_Kaab_Haak.Controller;
[ApiController]
[Route("WebAPI_Kaab_Haak/Brand")] 
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class BrandController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly IStoreFiles storeFiles;
    private readonly string container = "brandimage";

    public BrandController(AplicationdbContext context, IMapper mapper, IStoreFiles storeFiles){
        this.context = context;
        this.mapper = mapper;
        this.storeFiles = storeFiles;
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
    public async Task<ActionResult<Brand>> Post([FromForm] BrandCDTO brandCDTO){
        var exist = await context.Brand.AnyAsync(brand => brand.Name == brandCDTO.Name);
        if (exist)
        {
            return BadRequest("Brand exist");
        }

        var brand = mapper.Map<Brand>(brandCDTO);

        if (brandCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await brandCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(brandCDTO.Image.FileName);
                brand.Image = await storeFiles.SaveFile(content, extension, container,
                    brandCDTO.Image.ContentType);
            }
        }

        brand.CreateOn = DateTime.Now;
        brand.UpdateOn = DateTime.Now;

        context.Add(brand);
        await context.SaveChangesAsync();
        return Ok("Brand Created");
    }

    [HttpPut("{id:int}", Name = "UpdateBrand")]
    public async Task<ActionResult<Brand>> UpdateById(int id, [FromForm] BrandCDTO brandCDTO){
        
        var brandDB = await context.Brand.FirstOrDefaultAsync(x => x.Id == id);
        if (brandDB == null){
            return NotFound();
        }

        brandDB = mapper.Map(brandCDTO, brandDB);

        if (brandCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await brandCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(brandCDTO.Image.FileName);
                brandDB.Image = await storeFiles.SaveFile(content, extension, container,
                    brandCDTO.Image.ContentType);
            }
        }

        await context.SaveChangesAsync();
        return Ok("Brand Updated");
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<BrandPatchDTO> patchDocument){
        if(patchDocument == null){
            return BadRequest();
        }

        var BrandDB = await context.Brand.FirstOrDefaultAsync(x => x.Id == id);
        if (BrandDB == null){
            return NotFound("Brand not found");
        }

        var BrandDTO = mapper.Map<BrandPatchDTO>(BrandDB);

        patchDocument.ApplyTo(BrandDTO, ModelState);

        var isvalid = TryValidateModel(BrandDTO);
        if (!isvalid){
            return BadRequest(ModelState);
        }

        mapper.Map(BrandDTO, BrandDB);

        await context.SaveChangesAsync();
        return NoContent();
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