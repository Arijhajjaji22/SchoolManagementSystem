using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Models;
using ProjetBrima.Services;

[ApiController]
[Route("api/[controller]")]
public class EmploiController : ControllerBase // <-- Utilise ControllerBase pour API pure
{
    private readonly IEmploiService _emploiService;
    private readonly IMatiereService _matiereService;
    private readonly IProfesseurService _professeurService;
    private readonly ApplicationDbContext _context;

    public EmploiController(IEmploiService emploiService, IMatiereService matiereService, IProfesseurService professeurService, ApplicationDbContext context)
    {
        _emploiService = emploiService;
        _matiereService = matiereService;
        _professeurService = professeurService;
        _context = context;
    }

    [HttpPost("generer")]
    public async Task<IActionResult> GenererEmploi([FromBody] EmploiRequest request)
    {
        try
        {
            var emploi = await _emploiService.GenererEmploiAsync(request.ProfesseurId, request.MatiereNom, request.Classes);
            return Ok(emploi); // ✅
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var emplois = await _emploiService.GetAllAsync();
        return Ok(emplois);
    }

    [HttpGet("matieres")]
    public IActionResult GetMatieres()
    {
        var matieres = _matiereService.GetAllMatieres();
        return Ok(matieres);
    }

    [HttpGet("professeurs")]
    public async Task<IActionResult> GetProfesseursByMatiere([FromQuery] string matiere)
    {
        var allProfs = await _professeurService.GetAllProfesseursAsync();
        var filtered = allProfs.Where(p => p.Matiere == matiere).ToList();
        return Ok(filtered);
    }
    [HttpPost("upload-pdf")]
    public async Task<IActionResult> UploadPdf([FromBody] UploadPdfRequest dto)
    {
        var emplois = await _context.Emplois
            .Include(e => e.Professeur)
            .Include(e => e.Salle)
            .Include(e => e.Matiere)
            .Where(e => e.ProfesseurId == dto.ProfesseurId && e.DateDebutSemaine == dto.DateDebutSemaine)
            .ToListAsync();

        // Vérification si aucun emploi n'a été trouvé
        if (emplois == null || emplois.Count == 0)
            return NotFound("Emploi non trouvé.");

        // On prend le premier emploi trouvé, ou vous pouvez appliquer une logique plus avancée
        var emploi = emplois.FirstOrDefault();

        var pdf = new EmploisDuTempsPDF
        {
            ProfesseurId = emploi.ProfesseurId,
            ProfesseurNom = emploi.Professeur.Nom,
            ProfesseurPrenom = emploi.Professeur.Prenom,
            SalleId = emploi.SalleId,
            NumSalle = emploi.Salle.NumSalle,
            PdfBase64 = dto.PdfBase64,
            Classe = dto.Classe
        };

        _context.EmploisPDF.Add(pdf);
        await _context.SaveChangesAsync();

        return Ok("PDF enregistré avec succès !");
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetEmploiDetails(int id)
    {
        var creneaux = await _emploiService.GetCreneauxByEmploiIdAsync(id); // méthode qui retourne une liste
        if (creneaux == null || !creneaux.Any())
            return NotFound();

        return Ok(creneaux); // => retourne un tableau JSON
    }

    [HttpGet("pdfs")]
    public async Task<IActionResult> GetAllPdfs()
    {
        var pdfs = await _context.EmploisPDF
            .OrderByDescending(e => e.DateAjout)
            .ToListAsync();

        return Ok(pdfs);
    }

[HttpGet]
public async Task<IActionResult> GetEmploisProfesseur()
{
    var emplois = await _emploiService.GetEmploisDuProfAsync();
    return Ok(emplois);
}


}
public class UploadPdfRequest
{
    public int EmploiId { get; set; }
    public string PdfBase64 { get; set; }
    public int ProfesseurId { get; set; }
    public DateTime DateDebutSemaine { get; set; }
    public string Classe { get; set; }

}


public class EmploiRequest
{
    public int ProfesseurId { get; set; }
    public string MatiereNom { get; set; }
    public List<string> Classes { get; set; }

}
