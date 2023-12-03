using System.Text;
using FileApi.DataBaseFake;
using FileApi.UseCase.Interfaces;

namespace FileApi.UseCase;

public class FileUseCaseService : IFileUseCaseService
{
    public async Task CreateFileAsync(string fileContent)
    {
        await using var fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "new.txt"));
        var b = Encoding.Default.GetBytes(fileContent);
        await fs.WriteAsync(b);
    }

    public void CreateFile(string fileContent)
    {
        using var fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "new.txt"));
        var b = Encoding.Default.GetBytes(fileContent);
        fs.Write(b);
    }
    
    public Task<FileStream> GetFileAsync()
    {
        var fs = File.Open(Path.Combine(Directory.GetCurrentDirectory(), "test.txt"), FileMode.Open);
        return Task.FromResult(fs);
    }

    public async Task SaveFile(IFormFile file, string typeStorage)
    {
        if (typeStorage == "db")
        {
            // сохраним в бд
            var fs = file.OpenReadStream();
            var fileName = file.FileName.Split('.');
            var fileContent = new byte[fs.Length];
            var readAsync = await fs.ReadAsync(fileContent);

            FakeDataBase.Files.Add(new FileInDb
            {
                FileContent = fileContent,
                FileName = fileName[0],
                FileExtension = fileName[1]
            });
            // debug
            var d = 1;
        }

        if (typeStorage == "fileSystem")
        {
            // сохраним в файловой системе
            var fs = file.OpenReadStream();
            var fileContent = new byte[fs.Length];
            var readAsync = await fs.ReadAsync(fileContent);
            var stringContent = Encoding.Default.GetString(fileContent);
            await CreateFileAsync(stringContent);
        }

        if (typeStorage == "ftp")
        {
            // отправим на ftp сервер
            // нужен ftp сервер)
            // можно сделать имитацию с другим сервером локально
            // для ftp необходимо использовать сторонюю библиотеку
            // https://learn.microsoft.com/ru-ru/dotnet/core/compatibility/networking/6.0/webrequest-deprecated
            //var client = new HttpClient();
            //await client.PostAsync("localhost://", new StreamContent(file.OpenReadStream()));
        }
    }
}