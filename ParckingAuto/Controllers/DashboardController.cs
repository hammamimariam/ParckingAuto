using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.DTO;
using ParckingAuto.Services;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly VehiculeService _vehiculeService;
        private readonly ChauffeurService _chauffeurService;
        private readonly AlerteService _alerteService;
        private readonly MaintenanceService _maintenanceService;
        private readonly ParcAutoContext _context;

        public DashboardController(
            VehiculeService vehiculeService,
            ChauffeurService chauffeurService,
            AlerteService alerteService,
            MaintenanceService maintenanceService,
            ParcAutoContext context)
        {
            _vehiculeService = vehiculeService;
            _chauffeurService = chauffeurService;
            _alerteService = alerteService;
            _maintenanceService = maintenanceService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            var vehicules = await _vehiculeService.GetAllAsync();
            var chauffeurs = await _chauffeurService.GetAllAsync();
            var alertes = await _alerteService.GetAllAsync();
            var maintenances = await _maintenanceService.GetAllAsync();

            return Ok(new DashboardDto
            {
                TotalVehicules = vehicules.Count,
                TotalChauffeurs = chauffeurs.Count,
                TotalAlertes = alertes.Count,
                CoutTotal = maintenances.Sum(m => m.Cout)
            });
        }

        [HttpGet("nbVehicules")]
        public async Task<ActionResult<int>> GetNbVehicules()
        {
            var list = await _vehiculeService.GetAllAsync();
            return Ok(list.Count);
        }

        [HttpGet("nbChauffeurs")]
        public async Task<ActionResult<int>> GetNbChauffeurs()
        {
            var list = await _chauffeurService.GetAllAsync();
            return Ok(list.Count);
        }

        [HttpGet("nbAlertes")]
        public async Task<ActionResult<int>> GetNbAlertes()
        {
            var list = await _alerteService.GetAllAsync();
            return Ok(list.Count);
        }

        [HttpGet("coutMaintenance")]
        public async Task<ActionResult<decimal>> GetCoutMaintenance()
        {
            var sixMonthsAgo = DateTime.Today.AddMonths(-5);
            var total = await _context.Maintenances
                .Where(m => m.DateIntervention >= new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1))
                .SumAsync(m => m.Cout);
            return Ok(total);
        }

        [HttpGet("charts")]
        public async Task<ActionResult<DashboardChartsDto>> GetCharts()
        {
            var vehicules = await _vehiculeService.GetAllAsync();
            var idsEnMission = await _context.Mouvements
                .Where(m => m.DateRetour == null)
                .Select(m => m.VehiculeId)
                .Distinct()
                .CountAsync();
            var carburants = await _context.Carburants.ToListAsync();
            var maintenances = await _maintenanceService.GetAllAsync();

            var moisLabels = Enumerable.Range(0, 6)
                .Select(i => DateTime.Today.AddMonths(-5 + i))
                .Select(d => d.ToString("MMM yyyy"))
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

            var coutCarburantParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    return (double)carburants
                        .Where(c => c.DatePlein.Year == month.Year && c.DatePlein.Month == month.Month)
                        .Sum(c => c.Montant);
                })
                .ToList();

            var coutMaintenanceParMois = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var month = DateTime.Today.AddMonths(-5 + i);
                    return (double)maintenances
                        .Where(m => m.DateIntervention.Year == month.Year && m.DateIntervention.Month == month.Month)
                        .Sum(m => m.Cout);
                })
                .ToList();

            return Ok(new DashboardChartsDto
            {
                Mois = moisLabels,
                LitresParMois = litresParMois,
                CoutCarburantParMois = coutCarburantParMois,
                CoutMaintenanceParMois = coutMaintenanceParMois,
                VehiculesEnMission = idsEnMission,
                VehiculesAuParking = Math.Max(vehicules.Count - idsEnMission, 0),
                CoutMaintenanceTotal = (double)maintenances.Sum(m => m.Cout),
                CoutMaintenance6Mois = coutMaintenanceParMois.Sum(),
                CoutCarburant6Mois = coutCarburantParMois.Sum(),
                ConsommationCarburantTotal = litresParMois.Sum()
            });
        }
    }
}
