using Api.Models;

namespace Api.Interfaces;

public interface IUserManager
{
    public int CreateUser(CreateUserModel createUserModel);
    
    public Task<int> UpdateUserAsync(UpdateUserModel createUserModel);

    public UserModel GetUserModel(int userId);
}