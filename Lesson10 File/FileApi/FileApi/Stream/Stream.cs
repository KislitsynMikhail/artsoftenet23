using System.Text;

namespace FileApi.Stream;

public static class Stream
{
    public static async Task<MemoryStream> MemoryStreamExample()
    {
        var bytes = Encoding.Default.GetBytes("sss");
        // создаем memoryStream
        var stream = new MemoryStream(bytes);
        // Запишем в поток еще байтов
        await stream.WriteAsync(bytes);
        
        // поток поддерживает возможность позиционирования?
        var canSeek = stream.CanSeek;
        
        // Пустой метод)))
        stream.Flush();

        var a = await stream.ReadAsync(bytes.AsMemory(0, (int)stream.Length));

        return stream;
    }

    public static void FileStreamExample()
    {
        var bytes = Encoding.Default.GetBytes("sss");
        var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "test.txt"), FileMode.Open);

        var canSeek = stream.CanSeek;
        stream.Write(bytes);
        // debug
        var d = stream.SafeFileHandle;
        var b = stream.Length;
        
        // А здесь не пустой)
        stream.Flush();
    }

    public static void BufferedStreamExample()
    {
        //var stream = new BufferedStream();
        // Записать 100K в файл:
        File.WriteAllBytes ("myFile.bin", new byte [100000]);
        using var fs = File.OpenRead ("myFile.bin");
        using var bs = new BufferedStream (fs, 20000);
        bs.ReadByte();
        Console.WriteLine(fs.Position); // 20000
    }

    public static void ExampleFileStream()
    {
        using var s = new FileStream ("test.txt", FileMode.Create);
        Console.WriteLine (s.CanRead); // True
        Console.WriteLine (s.CanWrite); // True
        Console.WriteLine (s.CanSeek); // True
        s.WriteByte (101);
        s.WriteByte (102);
        byte[] block = { 1, 2, 3, 4, 5 };
        s.Write (block, 0, block.Length); // Записать блок из 5 байтов
        Console.WriteLine (s.Length); // 7
        Console.WriteLine (s.Position); // 7
        s.Position = 0; // Переместиться обратно в начало
        Console.WriteLine (s.ReadByte()); // 101
        Console.WriteLine (s.ReadByte()); // 102
        // Читать из потока в массив block:
        Console.WriteLine (s.Read (block, 0, block.Length)); // 5
        Console.WriteLine (s.Read (block, 0, block.Length)); // 0
    }
    
    public static async Task ExampleFileStreamAsync()
    {
        await using var s = new FileStream ("test.txt", FileMode.Create);
        byte[] block = { 1, 2, 3, 4, 5 };
        await s.WriteAsync (block, 0, block.Length); // Асинхронная запись
        s.Position = 0; // Переместиться в начало
        // Читать из потока в массив block:
        Console.WriteLine (await s.ReadAsync (block, 0, block.Length)); // 5
    }
}