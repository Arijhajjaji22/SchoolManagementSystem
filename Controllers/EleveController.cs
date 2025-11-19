using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Models;
using ProjetBrima.Services;
using Stripe;
using Stripe.Checkout;

namespace ProjetBrima.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EleveController : ControllerBase
    {
        private readonly IEleveService _eleveService;
        private readonly IPaymentService _paymentService;
        private readonly ApplicationDbContext _context;

        private readonly IAbsenceService _absenceService;
        public EleveController(IEleveService eleveService, IPaymentService paymentService, ApplicationDbContext context, IAbsenceService absenceService)
        {
            _eleveService = eleveService;
            _paymentService = paymentService;
            _context = context;
            _absenceService = absenceService;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEleve(int id)
        {
            var eleve = await _eleveService.GetEleveByIdAsync(id);
            if (eleve == null)
            {
                return NotFound();
            }
            return Ok(eleve);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEleves()
        {
            var eleves = await _eleveService.GetAllElevesAsync();
            return Ok(eleves);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEleve([FromForm] Eleve eleve, [FromForm] IFormFile bulletinFile)
        {
            if (eleve == null || bulletinFile == null)
            {
                return BadRequest("Élève ou fichier manquant");
            }

            try
            {
                await _eleveService.CreateEleveAsync(eleve, bulletinFile);

                // On retourne ici l'ID de l’élève créé
                return Ok(new { id = eleve.Id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

       

        [HttpPut("{id}")]
public async Task<IActionResult> UpdateEleve(int id, [FromForm] Eleve eleve, [FromForm] IFormFile bulletinFile)
{
    if (id != eleve.Id)
    {
        return BadRequest();
    }

    await _eleveService.UpdateEleveAsync(eleve, bulletinFile);
    return NoContent();
}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEleve(int id)
        {
            await _eleveService.DeleteEleveAsync(id);
            return NoContent();
        }

        [HttpGet("ayant-paye")]
        public async Task<IActionResult> GetElevesAyantPaye()
        {
            var elevesPayes = await _eleveService.GetElevesAyantPayéAsync();
            return Ok(elevesPayes);
        }
        [HttpGet("GetBulletin/{id}")]
        public IActionResult GetBulletin(int id)
        {
            var eleve = _context.Eleves.FirstOrDefault(e => e.Id == id);
            if (eleve == null || eleve.BulletinFile == null)
                return NotFound();

            return File(eleve.BulletinFile, "application/pdf", $"Bulletin_{eleve.Nom}_{eleve.Prenom}.pdf");
        }
        [HttpPut("MajInfosInstitutionnelles/{id}")]
        public async Task<IActionResult> MajInfosInstitutionnelles(int id, [FromBody] InfosInstitutionnellesDto infos)
        {
            var eleve = await _context.Eleves.FindAsync(id);
            if (eleve == null) return NotFound();

            if (!string.IsNullOrEmpty(eleve.EmailInstitutionnel) && !string.IsNullOrEmpty(eleve.MotDePasseInstitutionnel))
            {
                return Conflict("Email déjà généré.");
            }

            eleve.EmailInstitutionnel = infos.EmailInstitutionnel;
            eleve.MotDePasseInstitutionnel = infos.MotDePasse;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GetInfosInstitutionnelles/{eleveId}")]
        public IActionResult GetInfosInstitutionnelles(int eleveId)
        {
            var eleve = _context.Eleves
                .Where(e => e.Id == eleveId)
                .Select(e => new {
                    emailInstitutionnel = e.EmailInstitutionnel,
                    motDePasse = e.MotDePasseInstitutionnel
                })
                .FirstOrDefault();

            if (eleve == null)
                return NotFound();

            return Ok(eleve);
        }
        [HttpPost("loginEleve")]
        public async Task<IActionResult> SignInEleve([FromForm] string email, [FromForm] string password, [FromForm] string? returnUrl = "/Home/EleveDashboard")
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Veuillez remplir tous les champs." });
            }

            var eleve = _context.Eleves
                .FirstOrDefault(d => d.EmailInstitutionnel == email && d.MotDePasseInstitutionnel == password);

            if (eleve == null)
            {
                return Unauthorized(new { message = "Email ou mot de passe invalide." });
            }

            HttpContext.Session.SetInt32("eleveId", eleve.Id);

            // Rediriger vers l'URL spécifiée
            return Redirect(returnUrl); // retourne une redirection HTTP
        }

        [HttpGet("elevesParClasseEtMatiere")]
        public async Task<IActionResult> GetElevesParClasseEtMatiere(string classe)
        {
            var eleves = await _eleveService.GetElevesParClasseAsync(classe); // récupère les élèves

            var result = new List<object>();

            foreach (var eleve in eleves)
            {
                var absence = await _absenceService.GetDernierStatutAsync(eleve.Id); // récupère le dernier statut
                result.Add(new
                {
                    id = eleve.Id,
                    nom = eleve.Nom,
                    prenom = eleve.Prenom,
                    status = absence?.Status ?? "non_defini"
                });
            }

            return Ok(result);
        }


    }


}
