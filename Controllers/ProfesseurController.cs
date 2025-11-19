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
    public class ProfesseurController :ControllerBase
    {
        private readonly IProfesseurService _ProfesseurService;
        private readonly ApplicationDbContext _context;

        public ProfesseurController(IProfesseurService ProfesseurService, ApplicationDbContext context)
        {
            _ProfesseurService = ProfesseurService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfesseurs()
        {
            var Professeurs = await _ProfesseurService.GetAllProfesseursAsync();
            return Ok(Professeurs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfesseur(int id)
        {
            var Professeur = await _ProfesseurService.GetProfesseurByIdAsync(id);
            if (Professeur == null)
            {
                return NotFound();
            }
            return Ok(Professeur);
        }

        [HttpPost]
        public async Task<IActionResult> AddProfesseur([FromBody] Professeur Professeur)
        {
            if (Professeur == null)
            {
                return BadRequest("Les données sont manquantes ou incorrectes.");
            }

            // Validation des données si nécessaire
            if (string.IsNullOrEmpty(Professeur.Prenom) || string.IsNullOrEmpty(Professeur.Nom))
            {
                return BadRequest("Le prénom et le nom sont obligatoires.");
            }

            // Ajoutez votre logique pour ajouter un Professeur
            await _ProfesseurService.AddProfesseurAsync(Professeur);
            return CreatedAtAction(nameof(GetProfesseur), new { id = Professeur.Id }, Professeur);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfesseur(int id, [FromBody] Professeur Professeur)
        {
            if (id != Professeur.Id)
            {
                return BadRequest("ID de Professeur non valide.");
            }

            // Vérification si le Professeur existe
            var existingProfesseur = await _ProfesseurService.GetProfesseurByIdAsync(id);
            if (existingProfesseur == null)
            {
                return NotFound("Professeur non trouvé.");
            }

            // Mise à jour du Professeur
            await _ProfesseurService.UpdateProfesseurAsync(Professeur);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesseur(int id)
        {
            await _ProfesseurService.DeleteProfesseurAsync(id);
            return NoContent();
        }
        [HttpPost("loginProfesseur")]
        public async Task<IActionResult> SignInProfesseur([FromForm] string email, [FromForm] string password, [FromForm] string? returnUrl = "/Home/ProfesseurDashboard")
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Veuillez remplir tous les champs." });
            }

            var professeur = _context.Professeurs
                .FirstOrDefault(d => d.EmailInstitutionnel == email && d.MotDePasse == password);

            if (professeur == null)
            {
                return Unauthorized(new { message = "Email ou mot de passe invalide." });
            }

            HttpContext.Session.SetInt32("professeurId", professeur.Id);

            // Rediriger vers l'URL spécifiée
            return Redirect(returnUrl); // retourne une redirection HTTP
        }
        [HttpGet("pdfs")]
        public async Task<IActionResult> GetEmploisAvecClasse()
        {
            var result = await _context.EmploisPDF
                .Select(pdf => new EmploiProfesseurDto
                {
                    Id = pdf.Id,
                    ProfesseurNom = _context.Professeurs
                        .Where(p => p.Id == pdf.ProfesseurId)
                        .Select(p => p.Nom)
                        .FirstOrDefault(),
                    ProfesseurPrenom = _context.Professeurs
                        .Where(p => p.Id == pdf.ProfesseurId)
                        .Select(p => p.Prenom)
                        .FirstOrDefault(),
                    Classe = _context.Emplois
                        .Where(e => e.ProfesseurId == pdf.ProfesseurId)
                        .Select(e => e.Classe)
                        .FirstOrDefault(), // on récupère juste une classe
                    NumSalle = pdf.NumSalle,
                    DateAjout = pdf.DateAjout,
                    PdfBase64 = pdf.PdfBase64
                })
                .ToListAsync();

            return Ok(result);
        }

    }
}
