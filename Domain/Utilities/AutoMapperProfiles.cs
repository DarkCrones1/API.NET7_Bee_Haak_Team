using AutoMapper;
using Web_API_Bee_Haak.Entities;
using Web_API_Bee_Haak.DTOS;

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
        

        CreateMap<UserDataCDTO, DataUser>();
        CreateMap<DataUser, UserDataDTO>();


        //Mapeo PaymentMethod
        // CreateMap<PaymentMethodCDTO, PaymentMethod>();
        //CreateMap<PaymentMethod, PaymentMethodDTO>();

        //Mapeo ShoppingCart
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
}