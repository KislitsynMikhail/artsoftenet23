namespace FileApi.UseCase.Interfaces;

public interface IFileUseCaseService
{
    Task CreateFileAsync(string fileContent);
    
    void CreateFile(string fileContent);

    Task<FileStream> GetFileAsync();

    Task SaveFile(IFormFile file, string typeStorage);
}