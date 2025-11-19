using Microsoft.AspNetCore.Identity;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User AuthenticateAdmin(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Admin");

            if (user == null)
                return null;

            // Vérifie le mot de passe haché
            if (!VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, enteredPassword);

            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
        public Directeur Authentifier(string email, string motDePasse)
        {
            return _context.Directeurs.FirstOrDefault(d =>
                d.EmailInstitutionnel == email && d.MotDePasse == motDePasse);
        }
    }

}
