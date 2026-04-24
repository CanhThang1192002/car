using LogicBusiness.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OtoBackend.Controllers.admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, ShowroomManager, ShowroomSales")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardAdminService _service;

        public DashboardController(IDashboardAdminService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary(int days = 30)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Staff";
            var showroomIdClaim = User.FindFirst("ShowroomId")?.Value;
            int? showroomId = showroomIdClaim != null ? int.Parse(showroomIdClaim) : null;

            var dto = await _service.GetSummaryAsync(days, role, showroomId);
            return Ok(dto);
        }
    }
}

