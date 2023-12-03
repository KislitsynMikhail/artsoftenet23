using Api.Models;

namespace Api.Interfaces;

public interface INotificationService
{
    public Guid RequestToNs(UserModel userModel);
    
    public Guid RequestToNsV2(UserModel userModel);
}