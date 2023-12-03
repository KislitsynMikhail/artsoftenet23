namespace Api.Models;


public class CreateUserModel
{
    public string Name { get; init; }
    
    public int Age { get; init; }
    
    public string Password { get; init; }
}

public class UpdateUserModel
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int Age { get; init; }
    
    public string Password { get; init; }
}

public class UserModel
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int Age { get; init; }
    
    public string Password { get; init; }
}