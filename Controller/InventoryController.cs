using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.Controller.Base;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web_API_Bee_Haak.Data;
using Web_API_Bee_Haak.DTOS;

namespace Web_API_Bee_Haak.Controller;

[ApiController]
[Route("API_Bee_Haak/Inventory")]
[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public class InventoryController :ControllerBase
{
    private readonly AplicationdbContext context;
    private readonly IMapper mapper;

    public InventoryController(AplicationdbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<InventoryDTO>>> Get(){
        var Inventory = await context.Inventory.ToListAsync();
        return mapper.Map<List<InventoryDTO>>(Inventory);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<ProductDTO>>> Get(int id){
        var inventory = await context.Product.Include(InventoryDTO => InventoryDTO.Brand).Include(InventoryDTO => InventoryDTO.Category).Where(ProductDTO => ProductDTO.InventoryId == id).ToListAsync();

        return mapper.Map<List<ProductDTO>>(inventory);
    }

    [HttpPost]
    public async Task<ActionResult<Inventory>> Post([FromBody]InventoryCDTO inventoryCDTO){
        var Inventory = mapper.Map<Inventory>(inventoryCDTO);

        Inventory.CreateOn = DateTime.Now;
        Inventory.UpdateOn = DateTime.Now;
        context.Add(Inventory);
        await context.SaveChangesAsync();
        return Ok("Inventory Created");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Inventory>> Put(int id, [FromBody] InventoryDTO inventoryDTO){
        var exist = await context.Inventory.AnyAsync(Inventory => Inventory.Id == id);

        if (!exist)
        {
            return BadRequest("Inventody diesn't exist");
        }

        var inventory = mapper.Map<Inventory>(inventoryDTO);

        inventory.UpdateOn = DateTime.Now;
        context.Update(inventory);
        await context.SaveChangesAsync();
        return Ok("Inventory Updated");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Inventory>> Delete(int id){
        var inventory = await context.Inventory.FirstOrDefaultAsync(inventory => inventory.Id == id);
        if(inventory == null)
        {
            return BadRequest("Inventory doesn't exist");
        }

        context.Remove(inventory);
        await context.SaveChangesAsync();
        return Ok("Inventory Deleted");
    }
}

