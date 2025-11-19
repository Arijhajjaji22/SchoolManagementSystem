using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;
using System.Diagnostics;

namespace ProjetBrima.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPaymentService _paymentService;

        public HomeController(ILogger<HomeController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Contact()
        {
            // Vérifier si l'accès a été validé via le code d'accès
            if (HttpContext.Session.GetString("AccessCodePassed") != "true")
            {
                // Si le code d'accès n'est pas validé, rediriger vers la page de saisie du code
                return RedirectToAction("CodeAccess");
            }

            // Si l'utilisateur est un admin, rediriger vers le tableau de bord admin
            if (HttpContext.Session.GetString("userrole") == "Admin")
            {
                return RedirectToAction("AdminDashboard");
            }

            // Si aucune des conditions ci-dessus n'est remplie, afficher la vue par défaut
            return View();
        }

        public IActionResult CodeAccess()
        {
            return View();
        }

        // Cette action est protégée et nécessite que l'utilisateur soit authentifié
        public IActionResult AdminDashboard()
        {
            // Vérifiez si l'utilisateur est authentifié et s'il a le rôle admin
            if (HttpContext.Session.GetString("userrole") != "Admin")
            {
                // Si l'utilisateur n'est pas un admin, redirigez-le vers la page d'accueil
                return RedirectToAction("AccessDenied", "Home");
            }

            // Si l'utilisateur est un admin, affichez le tableau de bord
            return View();
        }


        public IActionResult AccessDenied()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult AddUser()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult Listedesutilisateurs()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult OtherUsers()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult AjouterResponsableEcole()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult ComptesDirecteur()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult AjouterProfesseur()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult ComptesProfesseur()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult ComptesPersonnel()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult AjouterComptesPersonnel()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult Inscription()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        public IActionResult paiement()
        {
            return View(); // Page qui explique que l'accès est refusé
        }
        [HttpGet]

        public async Task<IActionResult> Success([FromQuery] int eleveId)
        {

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Cancel([FromQuery] int eleveId)
        {

            return View();
        }
        public IActionResult Listeeleves()
        {

            return View();

        }
        public IActionResult DirecteurDashboard()
        {
            return View();
        }
        public IActionResult ListedesutilisateursDirecteur()
        {
            return View();
        }
        public IActionResult ListeelevesDirecteur()
        {
            return View();
        }
        public IActionResult ComptesPersonnelDirecteur() { return View(); }
        public IActionResult ComptesDirecteurDirecteur() { return View(); }
        public IActionResult ComptesProfesseurDirecteur() { return View(); }
        public IActionResult Gestiondesmatieres() { return View(); }
        public IActionResult GestiondesSalles() { return View(); }
        public IActionResult Emploisdutemps() { return View(); }
        public IActionResult ProfesseurDashboard() { return View(); }
        public IActionResult EleveDashboard() { return View(); }
        public IActionResult EmploisDuTempsprof()
        {
            return View();
        }
        public IActionResult GestiondesAbsences()
        {
            return View();
        }
        public IActionResult Gestiondesdemandesdeconge()
        {
            return View();
        }
        public IActionResult gestiondesnotes()
        {
            return View();
        }
    }
}