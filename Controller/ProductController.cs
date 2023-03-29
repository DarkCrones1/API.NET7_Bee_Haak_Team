using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.Data;
using Web_API_Kaab_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Web_API_Kaab_Haak.Utilities;
using Web_API_Kaab_Haak.Domain.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Web_API_Kaab_Haak.Services;

namespace Web_API_Kaab_Haak.Controller;
//Inventory/{InventoryId}
[ApiController]
[Route("WebAPI_Kaab_Haak/Product")]
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class ProductController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly IStoreFiles storeFiles;
    private readonly string container = "productimage";

    public ProductController(AplicationdbContext context, IMapper mapper, IStoreFiles storeFiles)
    {
        this.context = context;
        this.mapper = mapper;
        this.storeFiles = storeFiles;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<List<ProductDTO>> Get([FromQuery] PaginationDTO paginationDTO){
        var queryable = context.Product.AsQueryable();
        await HttpContext.InsertPaginationsData(queryable);
        var product = await queryable.OrderBy(product => product.Name).Page(paginationDTO).Include(product => product.Brand).Include(product => product.Category).ToListAsync();

        return mapper.Map<List<ProductDTO>>(product);
    }
    
    
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Get(int id){

        var product = await context.Product.Include(product => product.Brand).Include(product => product.Category).FirstOrDefaultAsync(product => product.Id == id);

        if(product == null)
        {
            return BadRequest("Product doesn't exist");
        }

        return mapper.Map<ProductDTO>(product);
    }

    [AllowAnonymous]
    [HttpGet("Name")]
    public async Task<IEnumerable<ProductDTO>> Get(String Name){
        var query = await context.Product.Where(ProductDTO => ProductDTO.Name.Contains(Name)).Include(ProductDTO => ProductDTO.Category).Include(ProductDTO => ProductDTO.Brand).ToListAsync();

        return mapper.Map<IEnumerable<ProductDTO>>(query);
    }

    [AllowAnonymous]
    [HttpGet("Category")]
    public async Task<IEnumerable<ProductDTO>> GetCat(String category){
        var product = await context.Product.Where(ProductDTO => ProductDTO.Category.Name.Contains(category)).Include(ProductDTO => ProductDTO.Category).Include(ProductDTO => ProductDTO.Brand).ToListAsync();

        return mapper.Map<IEnumerable<ProductDTO>>(product);
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> Post([FromForm] ProductCDTO productCDTO){
        var exist = await context.Product.AnyAsync(Product => Product.Name == productCDTO.Name);
        if (exist)
        {
            return BadRequest("Product Exist");
        }

        var product = mapper.Map<Product>(productCDTO);

        if (productCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await productCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(productCDTO.Image.FileName);
                product.Image = await storeFiles.SaveFile(content, extension, container,
                    productCDTO.Image.ContentType);
            }
        }

        product.CreateOn = DateTime.Now;
        product.UpdateOn = DateTime.Now;

        context.Add(product);
        await context.SaveChangesAsync();
        return Ok("Product Created");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Put(int id, [FromForm] ProductCDTO productCDTO){
        
        var productDB = await context.Product.FirstOrDefaultAsync(x => x.Id == id);
        if (productDB == null){
            return NotFound();
        }

        productDB = mapper.Map(productCDTO, productDB);

        if (productCDTO.Image != null){
            using (var memoryStream = new MemoryStream()){
                await productCDTO.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(productCDTO.Image.FileName);
                productDB.Image = await storeFiles.SaveFile(content, extension, container,
                    productCDTO.Image.ContentType);
            }
        }
        
        await context.SaveChangesAsync();
        return Ok("Product Updated");
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<ProductPatchDTO> patchDocument){
        if(patchDocument == null){
            return BadRequest();
        }

        var ProductDB = await context.Product.FirstOrDefaultAsync(x => x.Id == id);
        if(ProductDB == null){
            return NotFound("Product not found");
        }

        var ProductDTO = mapper.Map<ProductPatchDTO>(ProductDB);

        patchDocument.ApplyTo(ProductDTO, ModelState);

        var isvalid = TryValidateModel(ProductDTO);
        if (!isvalid){
            return BadRequest(ModelState);
        }

        mapper.Map(ProductDTO, ProductDB);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> Delete(int id){
        var product = await context.Product.FirstOrDefaultAsync(product => product.Id == id);
        if (product == null)
        {
            return BadRequest("Product doesn't exist");
        }

        context.Remove(product);
        await context.SaveChangesAsync();
        return Ok("Product Deleted");
    }
}