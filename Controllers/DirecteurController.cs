using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirecteurController : Controller
    {
        private readonly IDirecteurService _directeurService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public DirecteurController(IDirecteurService directeurService,IEmailService emailService, IUserService userService, ApplicationDbContext context)
        {
            _directeurService = directeurService;
            _emailService = emailService;
            _userService = userService;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetDirecteurs()

        {
            var directeurs = await _directeurService.GetAllDirecteursAsync();
            return Ok(directeurs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirecteur(int id)
        {
            var directeur = await _directeurService.GetDirecteurByIdAsync(id);
            if (directeur == null)
            {
                return NotFound();
            }
            return Ok(directeur);
        }

        [HttpPost]
        public async Task<IActionResult> AddDirecteur([FromBody] Directeur directeur)
        {
            if (directeur == null)
            {
                return BadRequest("Les données sont manquantes ou incorrectes.");
            }
            
            // Validation des données si nécessaire
            if (string.IsNullOrEmpty(directeur.Prenom) || string.IsNullOrEmpty(directeur.Nom))
            {
                return BadRequest("Le prénom et le nom sont obligatoires.");
            }

            // Ajoutez votre logique pour ajouter un directeur
            await _directeurService.AddDirecteurAsync(directeur);
            return CreatedAtAction(nameof(GetDirecteur), new { id = directeur.Id }, directeur);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDirecteur(int id, [FromBody] Directeur directeur)
        {
            if (id != directeur.Id)
            {
                return BadRequest("ID de directeur non valide.");
            }

            // Vérification si le directeur existe
            var existingDirecteur = await _directeurService.GetDirecteurByIdAsync(id);
            if (existingDirecteur == null)
            {
                return NotFound("Directeur non trouvé.");
            }

            // Mise à jour du directeur
            await _directeurService.UpdateDirecteurAsync(directeur);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirecteur(int id)
        {
            await _directeurService.DeleteDirecteurAsync(id);
            return NoContent();
        }
        [HttpPost("EnvoyerEmailBienvenue")]
        public async Task<IActionResult> EnvoyerEmailBienvenue([FromBody] EmailBienvenueDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmailPersonnel) ||
                string.IsNullOrWhiteSpace(dto.EmailInstitutionnel) ||
                string.IsNullOrWhiteSpace(dto.MotDePasse))
            {
                return BadRequest("Données manquantes.");
            }

            var subject = "Bienvenue à notre institution";
            var body = $@"
        Bonjour,

        Votre compte a été créé avec succès. Voici vos identifiants :

        📧 Email institutionnel : {dto.EmailInstitutionnel}
        🔐 Mot de passe : {dto.MotDePasse}

        Merci de vous connecter à notre plateforme.

        Bien cordialement,
        L'équipe RH
    ";

            try
            {
                await _emailService.SendEmailAsync(dto.EmailPersonnel, subject, body);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error si nécessaire
                return StatusCode(500, "Erreur lors de l'envoi de l'email : " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult SignInDirecteur(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }
        [HttpPost("loginDirecteur")]
        public async Task<IActionResult> SignInDirecteur([FromForm] string email, [FromForm] string password, [FromForm] string? returnUrl = "/Home/DirecteurDashboard")
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Veuillez remplir tous les champs." });
            }

            var directeur = _context.Directeurs
                .FirstOrDefault(d => d.EmailInstitutionnel == email && d.MotDePasse == password);

            if (directeur == null)
            {
                return Unauthorized(new { message = "Email ou mot de passe invalide." });
            }

            HttpContext.Session.SetInt32("directeurId", directeur.Id);

            // Rediriger vers l'URL spécifiée
            return Redirect(returnUrl); // retourne une redirection HTTP
        }


    }

}
