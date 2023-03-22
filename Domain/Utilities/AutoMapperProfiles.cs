using AutoMapper;
using Web_API_Kaab_Haak.Entities;
using Web_API_Kaab_Haak.DTOS;

namespace ApiVie.Domain.Utilities;
public class AutoMapperProfiles :Profile
{
    public AutoMapperProfiles()
    {
        // Mapeo Brand
        CreateMap<BrandCDTO, Brand>();
        CreateMap<Brand, BrandDTO>();

        // Mapeo Category
        CreateMap<CategoryCDTO, Category>();
        CreateMap<Category, CategoryDTO>();

        // Mapeo Product
        CreateMap<ProductCDTO, Product>();
        CreateMap<Product, ProductDTO>();

        //Mapeo Inventory
        CreateMap<InventoryCDTO, Inventory>();
        CreateMap<Inventory, InventoryDTO>();
        CreateMap<Inventory, InventoryDTOW>().ForMember(InventoryDTO => InventoryDTO.Stock, opcions => opcions.MapFrom(MapInventoryDTOProducts));

        //Mapeo Cart
        CreateMap<CartCDTO, Cart>();
        CreateMap<Cart, CartDTO>();
        CreateMap<Cart, CartDTOW>().ForMember(CartDTO => CartDTO.Items, opcions => opcions.MapFrom(MapCartDTOCartItems));

        //Mapeo CartItem
        CreateMap<CartItemCDTO, CartItem>();
        CreateMap<CartItem, CartItemDTO>();
        
        //Mapeo DataUser
        CreateMap<UserDataCDTO, UserData>();
        CreateMap<UserData, UserDataDTO>();


        //Mapeo PaymentMethod
        // CreateMap<PaymentMethodCDTO, PaymentMethod>();
        //CreateMap<PaymentMethod, PaymentMethodDTO>();

        //Mapeo Cart
        // CreateMap<ShoppingCartCDTO, Shoppingcart>();
        //CreateMap<Shoppingcart, ShoppingCartDTO>();

        //Mapeo Sucursal
        // CreateMap<SucursalCDTO, Sucursal>();
        // CreateMap<Sucursal, SucursalDTO>();
    }

    

    private List<ProductDTO> MapInventoryDTOProducts(Inventory inventory, InventoryDTO inventoryDTO){
        var result = new List<ProductDTO>();

        if(inventory.Stock == null){
            return result;
        }

        foreach (var Product in inventory.Stock){
            result.Add(new ProductDTO{
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price
            });
        }

        return result;
    }

    private List<CartItemDTO> MapCartDTOCartItems(Cart cart, CartDTO cartDTO){
        var result = new List<CartItemDTO>();

        if(cart.Items == null){
            return result;
        }

        foreach (var item in cart.Items){
            result.Add(new CartItemDTO{
                Id = item.Id,
                // CartId = item.CartId,
                // ProductId = item.ProductId,
                Price = item.Price,
                Quantity = item.Quantity
            });
        }

        return result;
    }
}