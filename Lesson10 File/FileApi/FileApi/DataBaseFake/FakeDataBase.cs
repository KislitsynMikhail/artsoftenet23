namespace FileApi.DataBaseFake;

public static class FakeDataBase
{
    public static List<FileInDb> Files = new();
}

public class FileInDb
{
    public byte[] FileContent { get; set; }
    
    public string FileName { get; set; }
    
    public string FileExtension { get; set; }
}