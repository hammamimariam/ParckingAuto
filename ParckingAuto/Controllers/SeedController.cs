using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ParcAutoContext _context;
        public SeedController(ParcAutoContext context)
        {
            _context = context;
        }

        // Endpoint to reset the database and re-seed all sample data!
        [HttpPost("reset")]
        public IActionResult ResetAndSeed()
        {
            try
            {
                DbInitializer.ResetAndSeed(_context);
                return Ok(new { message = "Database reset and re-seeded successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}