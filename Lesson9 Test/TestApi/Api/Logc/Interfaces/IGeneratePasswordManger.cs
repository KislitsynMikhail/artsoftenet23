namespace Api.Interfaces;

public interface IGeneratePasswordManger
{
    public string GetExamplePassword();

    public string GetHashByPassword(string password);
}