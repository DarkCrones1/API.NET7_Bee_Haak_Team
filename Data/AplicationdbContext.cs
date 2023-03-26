using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_API_Kaab_Haak.Entities;

namespace Web_API_Kaab_Haak.Data;
public class AplicationdbContext: IdentityDbContext
{
    public AplicationdbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<CartItem>().HasKey(CartItem => new {CartItem.ProductId, CartItem.CartId});
        modelBuilder.Entity<PaymentUser>().HasKey(PaymentUser => new {PaymentUser.UserId, PaymentUser.PaymentMethodId});
    }

    // public DbSet<User> User {get;set;}
    public DbSet<Brand> Brand {get;set;}
    public DbSet<Category> Category {get;set;}
    public DbSet<UserData> UserData {get;set;}
    public DbSet<Inventory> Inventory {get;set;}
    public DbSet<CartItem> CartItem {get;set;}
    public DbSet<Order> Order {get;set;}
    public DbSet<PaymentMethod> PaymentMethod {get;set;}
    public DbSet<PaymentUser> PaymentUser {get;set;}
    public DbSet<Product> Product {get;set;}
    public DbSet<Cart> Cart {get;set;}
}