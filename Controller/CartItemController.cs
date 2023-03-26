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
[Route("WebAPI_Kaab_Haak/CartItem")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CartItemController: ControllerBase{

    private readonly AplicationdbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public CartItemController(AplicationdbContext context, IMapper mapper, UserManager<IdentityUser> userManager){
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<CartItem>> Post([FromBody] CartItemCDTO cartItemCDTO){
        var exist = await context.CartItem.AnyAsync(CartItem => CartItem.ProductId == cartItemCDTO.ProductId);

        var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id;

        if (exist)
        {
            var cartItem = await context.CartItem.FirstOrDefaultAsync(cartItem => cartItem.ProductId == cartItemCDTO.ProductId);

            var product = await context.Product.FirstOrDefaultAsync(product => product.Id == cartItemCDTO.ProductId);

            var cart = await context.Cart.FirstOrDefaultAsync(cart => cart.UserId == userId);

            cartItem.CartId = cart.Id;

            cartItem.UpdateOn = DateTime.Now;

            cartItem.Price = product.Price;

            cartItem.Quantity += cartItemCDTO.Quantity;

            context.Update(cartItem);
            await context.SaveChangesAsync();

            return Ok("pieces added");
        }else{
            var cartItem = mapper.Map<CartItem>(cartItemCDTO);

            var cart = await context.Cart.FirstOrDefaultAsync(cart => cart.UserId == userId);
            var product = await context.Product.FirstOrDefaultAsync(product => product.Id == cartItemCDTO.ProductId);


            cartItem.Price = product.Price;
            cartItem.CartId = cart.Id;

            cartItem.CreateOn = DateTime.Now;
            cartItem.UpdateOn = DateTime.Now;
            context.Add(cartItem);
            await context.SaveChangesAsync();

            return Ok("Product Added");
        }

        // var cartItem = mapper.Map<CartItem>(cartItemCDTO);

        // cartItem.CreateOn = DateTime.Now;
        // cartItem.UpdateOn = DateTime.Now;
        // context.Add(cartItem);
        // await context.SaveChangesAsync();

        // var productCDTO = await context.Product.FirstOrDefaultAsync(product => product.Id == cartItemCDTO.ProductId);

        // var product = mapper.Map<Product>(productCDTO);

        // product.Quantity = product.Quantity-cartItemCDTO.Quantity;
        // context.Update(product);
        // await context.SaveChangesAsync();
        
    }
}