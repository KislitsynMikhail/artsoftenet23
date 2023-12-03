using System;
using Api;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace TestApi;


public class TestMock : IDisposable
{
    private readonly Mock<IGeneratePasswordManger> _generatePasswordManger;
    private readonly Mock<INotificationService> _notificationService;
    private IUserManager _userManager;
    
    public TestMock()
    {
        _generatePasswordManger = new Mock<IGeneratePasswordManger>();
        _notificationService =  new Mock<INotificationService>();
    }

    [Fact]
    public void Сreate_User_Success()
    {
        // SETUP
        var user = new CreateUserModel
        {
            Name = "Dima",
            Age = 23,
            Password = "",
        };
        
        Initialize();
        
        var res = _notificationService
            .Setup(x => x.RequestToNs(It.IsAny<UserModel>()))
            .Returns(Guid.NewGuid);

        #region ломающий мок
        // Если включить этот мок, тесты сломаются.
        /*_notificationService
            .Setup(x => x.RequestToNsV2(It.IsAny<UserModel>()))
            .Returns(Guid.NewGuid);*/
        #endregion
        
        // ACT
        var idone = _userManager.CreateUser(user);
        var idtwo = _userManager.CreateUser(user);

        // ASSER
        Assert.Equal(1, idone);
        Assert.Equal(2, idtwo);
        
        _notificationService.VerifyAll();
    }
    
    [Fact]
    public void Сreate_User_Success_Two()
    {
        // SETUP
        var user = new CreateUserModel
        {
            Name = "Dima",
            Age = 23,
            Password = "",
        };
        
        Initialize();
        
        _notificationService
            .Setup(x => x.RequestToNs(It.IsAny<UserModel>()))
            .Returns(Guid.NewGuid);
        
        // ACT
        var idone = _userManager.CreateUser(user);
        var idtwo = _userManager.CreateUser(user);

        // ASSER
        Assert.Equal(1, idone);
        Assert.Equal(2, idtwo);
    }
    
    private void Initialize()
    {
        _userManager = new UserManager(
            _generatePasswordManger.Object, 
            _notificationService.Object
        );
    }

    public void Dispose()
    {
    }
}