using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetBrima.DTO;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        // Enregistrer une absence ou présence
        [HttpPost]
        [Route("enregistrer")]
        public async Task<IActionResult> EnregistrerAbsence([FromBody] AbsenceDto absenceDto)
        {
            if (absenceDto == null || string.IsNullOrEmpty(absenceDto.Status))
            {
                return BadRequest("Les informations sont invalides.");
            }

            // Appeler le service pour enregistrer l'absence
            await _absenceService.EnregistrerAbsenceAsync(absenceDto.EleveId, absenceDto.MatiereId, absenceDto.Status);
            return Ok(new { message = "Absence enregistrée avec succès." });

        }

        // Récupérer les absences d'un élève
        [HttpGet]
        [Route("eleve/{eleveId}")]
        public async Task<IActionResult> GetAbsencesByEleve(int eleveId)
        {
            var absences = await _absenceService.GetAbsencesByEleveAsync(eleveId);
            return Ok(absences);
        }
    }

}
