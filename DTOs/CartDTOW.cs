namespace Web_API_Kaab_Haak.DTOS;
public class CartDTOW :CartDTO
{
    public List<CartItemDTO> Items {get;set;}
    public double TotalPrice {
        get{
            return Items.Sum(CartItem => CartItem.Price * CartItem.Quantity);
        }
    }
}