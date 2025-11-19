using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeCongeController : ControllerBase
    {
        private readonly IDemandeCongeService _demandeCongeService;

        public DemandeCongeController(IDemandeCongeService demandeCongeService)
        {
            _demandeCongeService = demandeCongeService;
        }

        // Endpoint pour récupérer toutes les demandes de congé
        [HttpGet]
        public async Task<IActionResult> GetDemandes()
        {
            var demandes = await _demandeCongeService.GetDemandesAsync();
            return Ok(demandes);
        }

        // Endpoint pour récupérer une demande spécifique par ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDemandeById(int id)
        {
            var demande = await _demandeCongeService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound("Demande de congé non trouvée");
            }
            return Ok(demande);
        }

        // Endpoint pour ajouter une nouvelle demande de congé
        [HttpPost]
        public async Task<IActionResult> AddDemande([FromBody] DemandeConge demande)
        {
            if (demande == null)
            {
                return BadRequest("Les données de la demande sont invalides");
            }

            var isAdded = await _demandeCongeService.AddDemandeAsync(demande);
            if (!isAdded)
            {
                return StatusCode(500, "Erreur lors de l'ajout de la demande");
            }
            return Ok("Demande de congé ajoutée avec succès");
        }

        // Endpoint pour mettre à jour le statut d'une demande (ex: Approuvé, Refusé)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatut(int id, [FromBody] string statut)
        {
            var isUpdated = await _demandeCongeService.UpdateDemandeStatutAsync(id, statut);
            if (!isUpdated)
            {
                return StatusCode(500, "Erreur lors de la mise à jour du statut de la demande");
            }
            return Ok("Statut de la demande mis à jour");
        }
        // Endpoint pour modifier complètement une demande existante
        [HttpPut("modifier/{id}")]
        public async Task<IActionResult> UpdateDemande(int id, [FromBody] DemandeConge demandeModifiee)
        {
            var demandeExistante = await _demandeCongeService.GetDemandeByIdAsync(id);
            if (demandeExistante == null)
            {
                return NotFound("Demande non trouvée");
            }

            // Mettre à jour les champs
            demandeExistante.NomEnseignant = demandeModifiee.NomEnseignant;
            demandeExistante.EmailEnseignant = demandeModifiee.EmailEnseignant;
            demandeExistante.DateDebut = demandeModifiee.DateDebut;
            demandeExistante.DateFin = demandeModifiee.DateFin;
            demandeExistante.Motif = demandeModifiee.Motif;
            demandeExistante.Statut = demandeModifiee.Statut;

            var isUpdated = await _demandeCongeService.UpdateDemandeAsync(demandeExistante);
            if (!isUpdated)
            {
                return StatusCode(500, "Erreur lors de la mise à jour de la demande");
            }

            return Ok("Demande mise à jour avec succès");
        }
        // Endpoint pour supprimer une demande
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemande(int id)
        {
            var isDeleted = await _demandeCongeService.DeleteDemandeAsync(id);
            if (!isDeleted)
            {
                return NotFound("Demande non trouvée ou erreur lors de la suppression");
            }
            return Ok("Demande supprimée avec succès");
        }


    }

}
