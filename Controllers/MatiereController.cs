using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatiereController : ControllerBase
    {
        private readonly IMatiereService _matiereService;
        private readonly IWebHostEnvironment _env;

        public MatiereController(IMatiereService matiereService, IWebHostEnvironment env)
        {
            _matiereService = matiereService;
            _env = env;
        }

        [HttpGet("{classe}")]
        public IActionResult GetByClasse(string classe)
        {
            var matieres = _matiereService.GetMatieresByClasse(classe);
            return Ok(matieres);
        }

        [HttpPost]
        public IActionResult Add([FromForm] Matiere matiere, IFormFile? programme)
        {
            if (programme != null && programme.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    programme.CopyTo(ms);
                    matiere.ProgrammePdf = ms.ToArray();
                }
            }

            _matiereService.AddMatiere(matiere);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] Matiere matiere, IFormFile? programme)
        {
            var existing = _matiereService.GetMatiereById(id);
            if (existing == null) return NotFound();

            existing.Nom = matiere.Nom;
            existing.Horaire = matiere.Horaire;
           

            if (programme != null && programme.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    programme.CopyTo(ms);
                    existing.ProgrammePdf = ms.ToArray();
                }
            }

            _matiereService.UpdateMatiere(existing);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _matiereService.DeleteMatiere(id);
            return Ok();
        }
        [HttpGet("download/{id}")]
        public IActionResult DownloadProgramme(int id)
        {
            var matiere = _matiereService.GetMatiereById(id);
            if (matiere == null || matiere.ProgrammePdf == null)
                return NotFound();

            return File(matiere.ProgrammePdf, "application/pdf", $"{matiere.Nom}_programme.pdf");
        }
        [HttpGet]
        public IActionResult GetAllMatieres()
        {
            var matieres = _matiereService.GetAllMatieres();
            return Ok(matieres);
        }



    }

}
