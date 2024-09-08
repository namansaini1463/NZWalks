using Microsoft.AspNetCore.Identity;

namespace WebApiNZwalks.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
