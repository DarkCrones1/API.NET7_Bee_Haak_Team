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

namespace Web_API_Kaab_Haak.Controller;

[ApiController]
[Route("WebAPI_Kaab_Haak/Cart")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CartController: ControllerBase{

    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public CartController(AplicationdbContext context, IMapper mapper, UserManager<IdentityUser> userManager){
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<List<CartDTO>>> Get(){
        var Cart = await context.Cart.ToListAsync();
        return mapper.Map<List<CartDTO>>(Cart);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<CartItemDTO>>> Get(int id){
        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var CartItem = await context.CartItem.Include(CartItem => CartItem.Cart).Include(CartItem => CartItem.Product.Brand).Include(CartItem => CartItem.Product.Category).Include(CartItem => CartItem.Cart).Where(CartItemDTO => CartItemDTO.CartId == id).ToListAsync();

        return mapper.Map<List<CartItemDTO>>(CartItem);
    }

    // [HttpGet("FinalCart")]
    // public async Task<ActionResult<CartDTOW>> Get(){
    //     var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
    //     var email = emailClaim.Value;
    //     var user = await userManager.FindByEmailAsync(email);
    //     var userId = user.Id;

    //     var finalCart = 
    // }

    [HttpPost]
    public async Task<ActionResult<Cart>> Post([FromBody]CartCDTO cartCDTO){

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        var Cart = mapper.Map<Cart>(cartCDTO);

        Cart.UserId = userId;
        Cart.CreateOn = DateTime.Now;
        Cart.UpdateOn = DateTime.Now;
        context.Add(Cart);
        await context.SaveChangesAsync();
        return Ok("Cart Created");
    }
}