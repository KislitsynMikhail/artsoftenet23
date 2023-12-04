using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Logic.Users;
using Microsoft.AspNetCore.Identity;

namespace Auth.Logic;

public class CustomPasswordPolicy : PasswordValidator<User>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
    {
        var result = await base.ValidateAsync(manager, user, password);
        var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();
 
        if (CheckDuplicates(password, 2))
        {
            errors.Add(new IdentityError
            {
                Description = "Password cannot contain username"
            });
        }
        
        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
    
    /// <inheritdoc />
    public bool CheckDuplicates(string password, int repeatingCharactersMax)
    {
        var count = 1;
        for (var i = 0; i < password.Length; i++)
        {
            // если послдений символ, то уже не с чем сравнивать
            if (i == password.Length - 1)
            {
                return false;
            }

            // если равный увеличиваем счетчик
            if (password[i] == password[i+1])
            {
                count += 1;
            }
            else // сбрасываем если не равно
            {
                count = 1;
            }

            // если предела достигли
            if (count == repeatingCharactersMax)
            {
                return true;
            }
        }

        return false;
    }
}