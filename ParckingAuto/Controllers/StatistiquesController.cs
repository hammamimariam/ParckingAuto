using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.DTO;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur,Gestionnaire")]
    public class StatistiquesController : ControllerBase
    {
        private readonly ParcAutoContext _context;

        public StatistiquesController(ParcAutoContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        [HttpGet]
        public async Task<ActionResult<StatistiquesDto>> Get([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var maintenancesQuery = _context.Maintenances.AsQueryable();
            var carburantsQuery = _context.Carburants.Include(c => c.Vehicule).AsQueryable();

            if (filter != null)
            {
                if (filter.Annee.HasValue)
                {
                    maintenancesQuery = maintenancesQuery.Where(m => m.DateIntervention.Year == filter.Annee.Value);
                    carburantsQuery = carburantsQuery.Where(c => c.DatePlein.Year == filter.Annee.Value);
                }
                if (filter.Mois.HasValue)
                {
                    maintenancesQuery = maintenancesQuery.Where(m => m.DateIntervention.Month == filter.Mois.Value);
                    carburantsQuery = carburantsQuery.Where(c => c.DatePlein.Month == filter.Mois.Value);
                }
                if (filter.VehiculeId.HasValue)
                {
                    maintenancesQuery = maintenancesQuery.Where(m => m.VehiculeId == filter.VehiculeId.Value);
                    carburantsQuery = carburantsQuery.Where(c => c.VehiculeId == filter.VehiculeId.Value);
                }
            }

            var maintenances = await maintenancesQuery.ToListAsync();
            var carburants = await carburantsQuery.ToListAsync();

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

        [HttpGet("export/carburant/csv")]
        public async Task<IActionResult> ExportCarburantCsv([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Carburants.Include(c => c.Vehicule).AsQueryable();
            ApplyCarburantFilters(ref query, filter);

            var carburants = await query
                .OrderByDescending(c => c.DatePlein)
                .Select(c => new CarburantExportDto
                {
                    DatePlein = c.DatePlein,
                    VehiculeImmatriculation = c.Vehicule != null ? c.Vehicule.Immatriculation : "",
                    Litres = (double)c.VolumeLitres,
                    Cout = (double)c.Montant,
                    Kilometrage = (double)c.Kilometrage
                })
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Date,Véhicule,Litres,Coût (DT),Kilométrage");
            foreach (var item in carburants)
            {
                csv.AppendLine($"{item.DatePlein:dd/MM/yyyy},{item.VehiculeImmatriculation},{item.Litres},{item.Cout},{item.Kilometrage}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "carburants.csv");
        }

        [HttpGet("export/carburant/pdf")]
        public async Task<IActionResult> ExportCarburantPdf([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Carburants.Include(c => c.Vehicule).AsQueryable();
            ApplyCarburantFilters(ref query, filter);

            var carburants = await query
                .OrderByDescending(c => c.DatePlein)
                .Select(c => new CarburantExportDto
                {
                    DatePlein = c.DatePlein,
                    VehiculeImmatriculation = c.Vehicule != null ? c.Vehicule.Immatriculation : "",
                    Litres = (double)c.VolumeLitres,
                    Cout = (double)c.Montant,
                    Kilometrage = (double)c.Kilometrage
                })
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Rapport Carburants")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Véhicule");
                                header.Cell().Element(CellStyle).Text("Litres");
                                header.Cell().Element(CellStyle).Text("Coût (DT)");
                                header.Cell().Element(CellStyle).Text("Kilométrage");

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3);
                                }
                            });

                            foreach (var item in carburants)
                            {
                                table.Cell().Element(CellStyle).Text(item.DatePlein.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(item.VehiculeImmatriculation);
                                table.Cell().Element(CellStyle).Text(item.Litres.ToString("0.00"));
                                table.Cell().Element(CellStyle).Text(item.Cout.ToString("0.00"));
                                table.Cell().Element(CellStyle).Text(item.Kilometrage.ToString("0"));

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            return File(pdfBytes, "application/pdf", "carburants.pdf");
        }

        [HttpGet("export/maintenance/csv")]
        public async Task<IActionResult> ExportMaintenanceCsv([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Maintenances.Include(m => m.Vehicule).AsQueryable();
            ApplyMaintenanceFilters(ref query, filter);

            var maintenances = await query
                .OrderByDescending(m => m.DateIntervention)
                .Select(m => new MaintenanceExportDto
                {
                    DateIntervention = m.DateIntervention,
                    VehiculeImmatriculation = m.Vehicule != null ? m.Vehicule.Immatriculation : "",
                    TypeIntervention = m.TypeIntervention,
                    Description = m.Description,
                    Cout = m.Cout
                })
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Date,Véhicule,Type intervention,Description,Coût (DT)");
            foreach (var item in maintenances)
            {
                csv.AppendLine($"{item.DateIntervention:dd/MM/yyyy},{item.VehiculeImmatriculation},{item.TypeIntervention},{item.Description},{item.Cout}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "maintenances.csv");
        }

        [HttpGet("export/maintenance/pdf")]
        public async Task<IActionResult> ExportMaintenancePdf([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Maintenances.Include(m => m.Vehicule).AsQueryable();
            ApplyMaintenanceFilters(ref query, filter);

            var maintenances = await query
                .OrderByDescending(m => m.DateIntervention)
                .Select(m => new MaintenanceExportDto
                {
                    DateIntervention = m.DateIntervention,
                    VehiculeImmatriculation = m.Vehicule != null ? m.Vehicule.Immatriculation : "",
                    TypeIntervention = m.TypeIntervention,
                    Description = m.Description,
                    Cout = m.Cout
                })
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Rapport Maintenances")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(120);
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Véhicule");
                                header.Cell().Element(CellStyle).Text("Type");
                                header.Cell().Element(CellStyle).Text("Description");
                                header.Cell().Element(CellStyle).Text("Coût (DT)");

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3);
                                }
                            });

                            foreach (var item in maintenances)
                            {
                                table.Cell().Element(CellStyle).Text(item.DateIntervention.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(item.VehiculeImmatriculation);
                                table.Cell().Element(CellStyle).Text(item.TypeIntervention);
                                table.Cell().Element(CellStyle).Text(item.Description);
                                table.Cell().Element(CellStyle).Text(item.Cout.ToString("0.00"));

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            return File(pdfBytes, "application/pdf", "maintenances.pdf");
        }

        [HttpGet("export/mouvements/csv")]
        public async Task<IActionResult> ExportMouvementsCsv([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Mouvements.Include(m => m.Vehicule).Include(m => m.Chauffeur).AsQueryable();
            ApplyMouvementFilters(ref query, filter);

            var mouvements = await query
                .OrderByDescending(m => m.DateDepart)
                .Select(m => new MouvementExportDto
                {
                    DateDebut = m.DateDepart,
                    DateFin = m.DateRetour,
                    VehiculeImmatriculation = m.Vehicule != null ? m.Vehicule.Immatriculation : "",
                    ChauffeurNomComplet = m.Chauffeur != null ? $"{m.Chauffeur.Prenom} {m.Chauffeur.Nom}" : "",
                    Description = m.Destination
                })
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Date début,Date fin,Véhicule,Chauffeur,Destination");
            foreach (var item in mouvements)
            {
                var dateFinStr = item.DateFin.HasValue ? item.DateFin.Value.ToString("dd/MM/yyyy") : "";
                csv.AppendLine($"{item.DateDebut:dd/MM/yyyy},{dateFinStr},{item.VehiculeImmatriculation},{item.ChauffeurNomComplet},{item.Description}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "mouvements.csv");
        }

        [HttpGet("export/mouvements/pdf")]
        public async Task<IActionResult> ExportMouvementsPdf([FromQuery] StatistiquesFilterDto? filter = null)
        {
            var query = _context.Mouvements.Include(m => m.Vehicule).Include(m => m.Chauffeur).AsQueryable();
            ApplyMouvementFilters(ref query, filter);

            var mouvements = await query
                .OrderByDescending(m => m.DateDepart)
                .Select(m => new MouvementExportDto
                {
                    DateDebut = m.DateDepart,
                    DateFin = m.DateRetour,
                    VehiculeImmatriculation = m.Vehicule != null ? m.Vehicule.Immatriculation : "",
                    ChauffeurNomComplet = m.Chauffeur != null ? $"{m.Chauffeur.Prenom} {m.Chauffeur.Nom}" : "",
                    Description = m.Destination
                })
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Rapport Mouvements")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(120);
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date début");
                                header.Cell().Element(CellStyle).Text("Date fin");
                                header.Cell().Element(CellStyle).Text("Véhicule");
                                header.Cell().Element(CellStyle).Text("Chauffeur");
                                header.Cell().Element(CellStyle).Text("Destination");

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3);
                                }
                            });

                            foreach (var item in mouvements)
                            {
                                table.Cell().Element(CellStyle).Text(item.DateDebut.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(item.DateFin.HasValue ? item.DateFin.Value.ToString("dd/MM/yyyy") : "");
                                table.Cell().Element(CellStyle).Text(item.VehiculeImmatriculation);
                                table.Cell().Element(CellStyle).Text(item.ChauffeurNomComplet);
                                table.Cell().Element(CellStyle).Text(item.Description);

                                IContainer CellStyle(IContainer container)
                                {
                                    return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            return File(pdfBytes, "application/pdf", "mouvements.pdf");
        }

        private void ApplyCarburantFilters(ref IQueryable<Models.Carburant> query, StatistiquesFilterDto? filter)
        {
            if (filter == null) return;
            if (filter.Annee.HasValue) query = query.Where(c => c.DatePlein.Year == filter.Annee.Value);
            if (filter.Mois.HasValue) query = query.Where(c => c.DatePlein.Month == filter.Mois.Value);
            if (filter.VehiculeId.HasValue) query = query.Where(c => c.VehiculeId == filter.VehiculeId.Value);
        }

        private void ApplyMaintenanceFilters(ref IQueryable<Models.Maintenance> query, StatistiquesFilterDto? filter)
        {
            if (filter == null) return;
            if (filter.Annee.HasValue) query = query.Where(m => m.DateIntervention.Year == filter.Annee.Value);
            if (filter.Mois.HasValue) query = query.Where(m => m.DateIntervention.Month == filter.Mois.Value);
            if (filter.VehiculeId.HasValue) query = query.Where(m => m.VehiculeId == filter.VehiculeId.Value);
        }

        private void ApplyMouvementFilters(ref IQueryable<Models.Mouvement> query, StatistiquesFilterDto? filter)
        {
            if (filter == null) return;
            if (filter.Annee.HasValue) query = query.Where(m => m.DateDepart.Year == filter.Annee.Value);
            if (filter.Mois.HasValue) query = query.Where(m => m.DateDepart.Month == filter.Mois.Value);
            if (filter.VehiculeId.HasValue) query = query.Where(m => m.VehiculeId == filter.VehiculeId.Value);
            if (filter.ChauffeurId.HasValue) query = query.Where(m => m.ChauffeurId == filter.ChauffeurId.Value);
        }
    }
}
