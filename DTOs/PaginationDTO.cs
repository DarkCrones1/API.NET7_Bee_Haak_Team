namespace Web_API_Bee_Haak.DTOS;
public class PaginationDTO
{
    public int Page {get;set;} = 1;
    private int recordsPage  = 10;
    private readonly int MaxElementByPage = 20;

    public int RecordsPage {get{return recordsPage;} set{recordsPage = (value > MaxElementByPage)? MaxElementByPage : value;}}
}