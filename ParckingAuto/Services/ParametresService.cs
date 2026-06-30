
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.DTO;
using ParckingAuto.Models;

namespace ParckingAuto.Services
{
    public class ParametresService
    {
        private readonly ParcAutoContext _context;

        public ParametresService(ParcAutoContext context) => _context = context;

        public async Task<Parametres> GetAsync()
        {
            var parametres = await _context.Parametres.FirstOrDefaultAsync();
            if (parametres != null) return parametres;

            parametres = new Parametres();
            _context.Parametres.Add(parametres);
            await _context.SaveChangesAsync();
            return parametres;
        }

        public async Task<Parametres> UpdateAsync(ParametresDto dto)
        {
            var parametres = await GetAsync();
            parametres.NotifVidange = dto.NotifVidange;
            parametres.NotifAssurance = dto.NotifAssurance;
            parametres.NotifVisiteTech = dto.NotifVisiteTech;
            parametres.NotifPermis = dto.NotifPermis;
            await _context.SaveChangesAsync();
            return parametres;
        }
    }
}
