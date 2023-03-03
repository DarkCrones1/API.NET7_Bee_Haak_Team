using Web_API_Bee_Haak.Entities;

namespace Web_API_Bee_Haak.DTOS;
public class InventoryDTOW :InventoryDTO
{
    public List<ProductDTO> Stock {get;set;}
}