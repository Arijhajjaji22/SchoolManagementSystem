using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IUserService
    {
      User AuthenticateAdmin(string email, string password);
        Directeur Authentifier(string email, string motDePasse);
        // User GetUserByEmail(string email);
    }
}
