using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.Entities.Base;
using Web_API_Bee_Haak.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Web_API_Bee_Haak.Controller.Base;

[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme,Policy = "Admin")]
public abstract class ApiControllerBase<T> :ControllerBase where T: BaseEntity
{
    private readonly AplicationdbContext dbContext;
    private readonly ILogger<T> _logger;

    protected DbSet<T> entities
    {
        get{
            return dbContext.Set<T>();
        }
    }
    public ApiControllerBase(AplicationdbContext dbContext, ILogger<T> Logger)
    {
        this.dbContext = dbContext;
        _logger = Logger;
    }

    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<T>>> Get(){
        var list = await entities.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<T>> GetById(int id){
        var entity = await entities.FindAsync(id);
        if (entity is null)
        {
            return NotFound($"Not found entity with id {id}");
        }
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<T>> PostT(T entity){
        entity.CreateOn = DateTime.Now;
        entity.UpdateOn = DateTime.Now;

        await entities.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        
        return CreatedAtAction(
            nameof(PostT),
            new {id = entity.Id},
            entity
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<T>> UpdateT(int id, T entity){
        if (entity == null)
        {
            return BadRequest("Entity can't be null");
        }
        if (id != entity.Id)
        {
            return BadRequest("Id not found");
        }
        entity.UpdateOn = DateTime.Now;
        entities.Update(entity);
        await dbContext.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<T>> Delete(int id){
        var entity = await entities.FindAsync(id);
        if (entity == null)
        {
            return new NotFoundObjectResult($"entity not found with id: {id}");
        }

        entities.Remove(entity);
        await dbContext.SaveChangesAsync();
        return Ok(entity);
    }
}