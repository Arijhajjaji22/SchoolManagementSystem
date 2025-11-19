using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class EmploiService : IEmploiService
    {
        private readonly ApplicationDbContext _context;

        public EmploiService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmploiDuTemps>> GetAllAsync()
        {
            return await _context.Emplois
                .Include(e => e.Professeur)
                .Include(e => e.Matiere)
                .Include(e => e.Salle)
                .ToListAsync();
        }
        public async Task<List<EmploiDuTemps>> GenererEmploiAsync(int professeurId, string matiereNom, List<string> classes = null)
        {
            // Récupérer la matière et le professeur
            var matiere = await _context.Matiere
                .FirstOrDefaultAsync(m => m.Nom.ToLower() == matiereNom.ToLower());

            var prof = await _context.Professeurs.FindAsync(professeurId);

            if (matiere == null || prof == null)
                throw new Exception("Professeur ou matière non trouvée");

            // Liste de tous les créneaux disponibles
            string[] horaires = {
        "Lundi 08h00-10h00", "Lundi 10h00-12h00", "Mardi 08h00-10h00", "Mardi 10h00-12h00",
        "Mercredi 08h00-10h00", "Mercredi 10h00-12h00", "Jeudi 08h00-10h00", "Jeudi 10h00-12h00",
        "Vendredi 08h00-10h00", "Vendredi 10h00-12h00", "Lundi 14h00-16h00", "Lundi 16h00-18h00",
        "Mardi 14h00-16h00", "Mardi 16h00-18h00", "Mercredi 14h00-16h00", "Mercredi 16h00-18h00",
        "Jeudi 14h00-16h00", "Jeudi 16h00-18h00", "Vendredi 14h00-16h00", "Vendredi 16h00-18h00",
        "Samedi 08h00-10h00", "Samedi 10h00-13h00"
    };

            // Regrouper les horaires par jour
            var horairesParJour = horaires
                .GroupBy(h => h.Split(' ')[0]) // "Lundi", "Mardi", etc.
                .ToDictionary(g => g.Key, g => g.ToList());

            var dispoSalle = await _context.Salles
                .Where(s => s.Disponibilite == "Disponible")
                .ToListAsync();

            List<EmploiDuTemps> result = new List<EmploiDuTemps>();

            int totalHeures = matiere.Horaire;
            int dureeSeance = 2;
            int nbSeances = (int)Math.Ceiling((double)totalHeures / dureeSeance);

            int semaines = 12;
            int seancesParSemaine = (int)Math.Ceiling((double)nbSeances / semaines);

            int seancesAjoutees = 0;
            DateTime semaineCourante = DateTime.Now.Date;

            for (int semaine = 0; semaine < semaines && seancesAjoutees < nbSeances; semaine++)
            {
                int seancesCetteSemaine = 0;

                foreach (var jour in horairesParJour.Keys)
                {
                    if (seancesCetteSemaine >= seancesParSemaine)
                        break;

                    bool matierePlanifieeCeJour = false;

                    foreach (var horaire in horairesParJour[jour])
                    {
                        if (matierePlanifieeCeJour)
                            break; // saut si la matière est déjà planifiée ce jour-là

                        bool profOccupe = await _context.Emplois
                            .AnyAsync(e => e.ProfesseurId == professeurId && e.Horaire == horaire && e.DateDebutSemaine == semaineCourante);

                        if (profOccupe)
                            continue;

                        foreach (var salle in dispoSalle)
                        {
                            if (seancesAjoutees >= nbSeances)
                                break;

                            var classesToCheck = classes ?? new List<string> { "Classe générique" };

                            foreach (var classe in classesToCheck)
                            {
                                bool salleOccupee = await _context.Emplois
                                    .AnyAsync(e => e.SalleId == salle.Id && e.Horaire == horaire && e.DateDebutSemaine == semaineCourante);

                                bool classeOccupee = await _context.Emplois
                                    .AnyAsync(e => e.Classe == classe && e.Horaire == horaire && e.DateDebutSemaine == semaineCourante);

                                bool combinaisonSalleClasseOccupee = await _context.Emplois
                                    .AnyAsync(e =>
                                        e.SalleId == salle.Id &&
                                        e.Classe == classe &&
                                        e.Horaire == horaire &&
                                        e.DateDebutSemaine == semaineCourante &&
                                        e.MatiereId != matiere.Id);

                                bool doublonExact = await _context.Emplois
                                    .AnyAsync(e =>
                                        e.MatiereId == matiere.Id &&
                                        e.SalleId == salle.Id &&
                                        e.Classe == classe &&
                                        e.Horaire == horaire &&
                                        e.DateDebutSemaine == semaineCourante);

                                if (!salleOccupee && !classeOccupee && !combinaisonSalleClasseOccupee && !doublonExact)
                                {
                                    var emploi = new EmploiDuTemps
                                    {
                                        ProfesseurId = professeurId,
                                        MatiereId = matiere.Id,
                                        Classe = classe,
                                        SalleId = salle.Id,
                                        DateDebutSemaine = semaineCourante,
                                        Horaire = horaire
                                    };

                                    _context.Emplois.Add(emploi);
                                    await _context.SaveChangesAsync();

                                    var emploiAvecDetails = await _context.Emplois
                                        .Include(e => e.Salle)
                                        .Include(e => e.Professeur)
                                        .Include(e => e.Matiere)
                                        .OrderByDescending(e => e.Id)
                                        .FirstOrDefaultAsync(e =>
                                            e.ProfesseurId == professeurId &&
                                            e.MatiereId == matiere.Id &&
                                            e.Horaire == emploi.Horaire &&
                                            e.DateDebutSemaine == semaineCourante);

                                    result.Add(emploiAvecDetails);
                                    seancesAjoutees++;
                                    seancesCetteSemaine++;
                                    matierePlanifieeCeJour = true; // Marque la journée comme planifiée
                                    break;
                                }
                            }
                        }
                    }
                }

                semaineCourante = semaineCourante.AddDays(7); // semaine suivante
            }

            if (result.Count == 0)
                throw new Exception("Aucune combinaison disponible respectant les contraintes.");

            return result;
        }

        public async Task<EmploiDuTemps?> GetByIdAsync(int id)
        {
            return await _context.Emplois.FindAsync(id);
        }


        public async Task UpdateAsync(EmploiDuTemps emploi)
        {
            _context.Emplois.Update(emploi);
            await _context.SaveChangesAsync();
        }
        public async Task<List<EmploiDuTemps>> GetCreneauxByEmploiIdAsync(int emploiId)
        {
            return await _context.Emplois
                .Where(e => e.Id == emploiId)
                .Include(e => e.Professeur)
                .Include(e => e.Matiere)
                .Include(e => e.Salle)
                .ToListAsync();
        }

        public async Task<bool> SalleEstDisponibleAsync(int salleId, string horaire)
        {
            return !await _context.Emplois
                .AnyAsync(e => e.SalleId == salleId && e.Horaire == horaire);
        }
        public async Task<List<EmploisDuTempsPDF>> GetEmploisDuProfAsync()
        {
            var emploisGroupes = await _context.EmploisPDF
                .GroupBy(e => new { e.ProfesseurId, e.ProfesseurNom, e.ProfesseurPrenom })
                .Select(g => new EmploisDuTempsPDF
                {
                    ProfesseurId = g.Key.ProfesseurId,
                    ProfesseurNom = g.Key.ProfesseurNom,
                    ProfesseurPrenom = g.Key.ProfesseurPrenom,
                    Classe = string.Join(", ", g.Select(e => e.Classe).Distinct()),
                    SalleId = g.First().SalleId, // ou choisir la logique que tu veux pour la salle
                    NumSalle = g.First().NumSalle,
                    DateAjout = g.Min(e => e.DateAjout),
                    PdfBase64 = g.First().PdfBase64
                })
                .ToListAsync();

            return emploisGroupes;
        }

    }
}