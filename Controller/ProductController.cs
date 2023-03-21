using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Data;
using Web_API_Bee_Haak.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Web_API_Bee_Haak.Utilities;
using Web_API_Bee_Haak.Domain.Utilities;

namespace Web_API_Bee_Haak.Controller;
//Inventory/{InventoryId}
[ApiController]
[Route("API_Bee_Haak/Product")]
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class ProductController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;

    public ProductController(AplicationdbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
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
    public async Task<ActionResult<Product>> Post([FromBody] ProductCDTO productCDTO){
        var exist = await context.Product.AnyAsync(Product => Product.Name == productCDTO.Name);
        if (exist)
        {
            return BadRequest("Product Exist");
        }

        var product = mapper.Map<Product>(productCDTO);

        product.CreateOn = DateTime.Now;
        product.UpdateOn = DateTime.Now;

        context.Add(product);
        await context.SaveChangesAsync();
        return Ok("Product Created");
    }

    
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Put(int id, [FromBody] ProductCDTO productCDTO){
        var exist = await context.Product.AnyAsync(product => product.Id == id);

        if(!exist)
        {
            return BadRequest("Product doesn't exist");
        }

        var product = mapper.Map<Product>(productCDTO);
        product.Id = id;

        product.UpdateOn = DateTime.Now;
        context.Update(product);
        await context.SaveChangesAsync();
        return Ok("Product Updated");
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