using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.BLL.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, IList<string> roles);
}