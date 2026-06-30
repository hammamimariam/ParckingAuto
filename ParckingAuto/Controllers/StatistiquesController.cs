using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.DTO;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur,Gestionnaire")]
    public class StatistiquesController : ControllerBase
    {
        private readonly ParcAutoContext _context;

        public StatistiquesController(ParcAutoContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<StatistiquesDto>> Get()
        {
            var maintenances = await _context.Maintenances.ToListAsync();
            var carburants = await _context.Carburants.Include(c => c.Vehicule).ToListAsync();

            var moisLabels = Enumerable.Range(0, 6)
                .Select(i => DateTime.Today.AddMonths(-5 + i))
                .Select(d => d.ToString("MMM yyyy"))
                .ToList();

            var coutParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    return maintenances
                        .Where(m => m.DateIntervention.Year == month.Year && m.DateIntervention.Month == month.Month)
                        .Sum(m => m.Cout);
                })
                .ToList();

            var coutCarburantParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    return carburants
                        .Where(c => c.DatePlein.Year == month.Year && c.DatePlein.Month == month.Month)
                        .Sum(c => c.Montant);
                })
                .ToList();

            var litresParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    return carburants
                        .Where(c => c.DatePlein.Year == month.Year && c.DatePlein.Month == month.Month)
                        .Sum(c => (double)c.VolumeLitres);
                })
                .ToList();

            var consoMoyenneParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    var monthFuel = carburants
                        .Where(c => c.DatePlein.Year == month.Year && c.DatePlein.Month == month.Month)
                        .GroupBy(c => c.VehiculeId)
                        .Select(g =>
                        {
                            var ordered = g.OrderBy(c => c.Kilometrage).ToList();
                            if (ordered.Count < 2) return 0.0;
                            var litres = ordered.Sum(c => (double)c.VolumeLitres);
                            var km = ordered.Last().Kilometrage - ordered.First().Kilometrage;
                            return km > 0 ? (litres * 100) / km : 0.0;
                        })
                        .Where(x => x > 0)
                        .ToList();

                    return monthFuel.Count > 0 ? Math.Round(monthFuel.Average(), 2) : 0.0;
                })
                .ToList();

            var consommationParVehicule = carburants
                .GroupBy(c => c.VehiculeId)
                .Select(g =>
                {
                    var ordered = g.OrderBy(c => c.Kilometrage).ToList();
                    if (ordered.Count < 2)
                        return null;

                    var litres = ordered.Sum(c => (double)c.VolumeLitres);
                    var km = ordered.Last().Kilometrage - ordered.First().Kilometrage;
                    var moyenne = km > 0 ? (litres * 100) / km : 0;

                    return new ConsommationVehiculeDto
                    {
                        Immatriculation = ordered.First().Vehicule?.Immatriculation ?? $"Véhicule {g.Key}",
                        ConsommationMoyenne = Math.Round(moyenne, 2)
                    };
                })
                .Where(x => x != null)
                .Cast<ConsommationVehiculeDto>()
                .ToList();

            return Ok(new StatistiquesDto
            {
                Mois = moisLabels,
                CoutMaintenanceParMois = coutParMois,
                CoutCarburantParMois = coutCarburantParMois,
                LitresParMois = litresParMois,
                ConsommationMoyenneParMois = consoMoyenneParMois,
                ConsommationParVehicule = consommationParVehicule
            });
        }
    }
}
