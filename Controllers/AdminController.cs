// Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Services;
using ProjetBrima.Models;

namespace ProjetBrima.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEleveService _eleveService;

        public AdminController(IUserService userService,IEleveService eleveService)
        {
            _userService = userService;
            _eleveService = eleveService;
        }

        [HttpGet]
        public IActionResult SignInAdmin(string returnUrl)
        {
            // Passer l'URL de retour si elle existe
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }
        [HttpPost("login")]
        public IActionResult SignInAdmin(string email, string password, string returnUrl)
        {
            var user = _userService.AuthenticateAdmin(email, password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Email ou mot de passe invalide.";
                return View();
            }

            // Authentification réussie, enregistrer l'utilisateur dans la session
            HttpContext.Session.SetString("userrole", user.Role);

            // Si une URL de retour existe, rediriger l'utilisateur vers cette URL
            return Redirect(returnUrl ?? "/Home/AdminDashboard");  // Rediriger vers le tableau de bord admin
        }
        // Action de déconnexion
        [HttpPost]
        public IActionResult Logout()
        {
            // Supprimer les informations de session
            HttpContext.Session.Clear();

            // Rediriger vers la page d'accueil ou une page de connexion
            return RedirectToAction("Contact", "Home");
        }
        // Vérifie le code d'accès saisi
        [HttpPost("codeaccess")]
        public IActionResult CodeAccess(string accessCode)
        {
            // Vérifier le nombre de tentatives échouées
            int failedAttempts = HttpContext.Session.GetInt32("FailedAttempts") ?? 0;

            bool isLockedOut = false;
            string lockoutMessage = null;

            if (failedAttempts >= 2)
            {
                // Vérifier si l'utilisateur est toujours dans la période de verrouillage
                byte[] lockoutEndBytes = HttpContext.Session.Get("LockoutEndTime");

                if (lockoutEndBytes != null)
                {
                    DateTime lockoutEndTime = DateTime.FromBinary(BitConverter.ToInt64(lockoutEndBytes, 0));

                    if (lockoutEndTime > DateTime.Now)
                    {
                        // L'utilisateur est encore verrouillé
                        isLockedOut = true;
                        TimeSpan remainingLockoutTime = lockoutEndTime - DateTime.Now;
                        lockoutMessage = "Vous avez dépassé le nombre de tentatives autorisées. Vous pouvez réessayer dans 10 minute.";
                    }
                    else
                    {
                        // Si la période de verrouillage est terminée, réinitialiser les tentatives échouées
                        HttpContext.Session.SetInt32("FailedAttempts", 0);
                        HttpContext.Session.Remove("LockoutEndTime");
                    }
                }
            }

            // Code d'accès correct
            string correctCode = "1234"; // Exemple du code à hacher ou lire depuis un stockage sécurisé

            if (accessCode == correctCode)
            {
                // Réinitialiser les tentatives échouées
                HttpContext.Session.SetInt32("FailedAttempts", 0);

                // Si le code est correct, enregistrer la session
                HttpContext.Session.SetString("AccessCodePassed", "true");

                return RedirectToAction("Contact", "Home");
            }
            else
            {
                // Augmenter le nombre de tentatives échouées
                HttpContext.Session.SetInt32("FailedAttempts", failedAttempts + 1);

                if (failedAttempts + 1 >= 3)
                {
                    // Activer un verrouillage pendant 1 minute après 3 tentatives échouées
                    DateTime lockoutEndTime = DateTime.Now.AddMinutes(1);
                    HttpContext.Session.Set("LockoutEndTime", BitConverter.GetBytes(lockoutEndTime.ToBinary()));

                    lockoutMessage = "Vous avez dépassé le nombre de tentatives autorisées. Vous pouvez réessayer dans 1 minute.";
                }
                else
                {
                    // Afficher un message d'erreur normal si moins de 3 tentatives
                    lockoutMessage = "Code d'accès incorrect. Veuillez réessayer.";
                }
            }

            // Passer l'état du verrouillage et le message à la vue
            ViewBag.IsLockedOut = isLockedOut;
            ViewBag.LockoutMessage = lockoutMessage;
            return View("~/Views/Home/CodeAccess.cshtml");
        }
        [HttpGet("eleves-payes")]
        public async Task<IActionResult> GetElevesAyantPayé()
        {
            var elevesPayes = await _eleveService.GetElevesAyantPayéAsync();
            return Ok(elevesPayes);
        }

    }
}
