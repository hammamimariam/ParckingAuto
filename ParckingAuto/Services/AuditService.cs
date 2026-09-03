using System.Text.Json;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class AuditService
    {
        private readonly AuditRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AuditRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(int? userId, string actionType, string tableName, string? recordId = null, object? oldValues = null, object? newValues = null)
        {
            var audit = new Audit
            {
                UtilisateurId = userId,
                ActionType = actionType,
                TableName = tableName,
                RecordId = recordId,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                Timestamp = DateTime.Now,
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString()
            };

            await _repo.AddAsync(audit);
        }

        public Task<List<Audit>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Audit?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    }
}
