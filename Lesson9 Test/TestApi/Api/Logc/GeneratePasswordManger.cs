using Api.Interfaces;

namespace Api;

public class GeneratePasswordManger : IGeneratePasswordManger
{
    private static Random random = new Random();
    
    public string GetExamplePassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string GetHashByPassword(string password)
    {
        return CreateMD5(password);
    }

    private static string CreateMD5(string input)
    {
        using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}