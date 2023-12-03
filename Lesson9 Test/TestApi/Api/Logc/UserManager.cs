using Api.Interfaces;
using Api.Models;

namespace Api;

public class UserManager : IUserManager
{
    public List<UserModel> UserModels = new List<UserModel>();
    
    private readonly IGeneratePasswordManger _generatePasswordManger;
    private readonly INotificationService _notificationService;

    public UserManager(IGeneratePasswordManger generatePasswordManger, INotificationService notificationService)
    {
        _generatePasswordManger = generatePasswordManger;
        _notificationService = notificationService;
    }

    private void ValidateUserModel(UserModel userModel)
    {
        if (userModel.Age < 18)
        {
            throw new Exception();
        }
    }
    
    public int CreateUser(CreateUserModel createUserModel)
    {
        var password = string.IsNullOrWhiteSpace(createUserModel.Password)
            ? _generatePasswordManger.GetExamplePassword() 
            : createUserModel.Password;
        
        var hash = _generatePasswordManger.GetHashByPassword(password);
        var user = new UserModel()
        {
            Id = UserModels.Count + 1,
            Name = createUserModel.Name,
            Age = createUserModel.Age,
            Password = hash
        };
        ValidateUserModel(user);
        UserModels.Add(user);
        if (string.IsNullOrWhiteSpace(createUserModel.Password))
        {
            _notificationService.RequestToNs(user);
        }
       
        return user.Id;
    }

    public async Task<int> UpdateUserAsync(UpdateUserModel createUserModel)
    {
        throw new NotImplementedException();
    }

    public UserModel GetUserModel(int userId)
    {
        return UserModels.First(value => value.Id == userId);
    }
}