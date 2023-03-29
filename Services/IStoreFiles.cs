namespace Web_API_Kaab_Haak.Services;

public interface IStoreFiles
{
    Task<string> SaveFile(byte[] content , string extension, string container, string ContentType);
    Task<string> EditFile(byte[] content , string extension, string container, string route, string ContentType);
    Task DeleteFile(string route, string container);
}