using LogicBusiness.DTOs;
using LogicBusiness.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OtoBackend.Controllers.admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, ShowroomManager, ShowroomSales")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderAdminService _adminService;

        public OrdersController(IOrderAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 10, string? search = null, string? status = null, string? paymentStatus = null)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var showroomIdClaim = User.FindFirst("ShowroomId")?.Value;
            int? showroomId = showroomIdClaim != null ? int.Parse(showroomIdClaim) : null;

            var result = await _adminService.GetOrdersForAdminAsync(page, pageSize, search, status, paymentStatus, role ?? "Staff", showroomId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderAdminDto dto)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var showroomIdClaim = User.FindFirst("ShowroomId")?.Value;
            int? showroomId = showroomIdClaim != null ? int.Parse(showroomIdClaim) : null;

            var result = await _adminService.CreateOrderAsync(dto, role ?? "Staff", showroomId);
            if (result.Success) return Ok(new { message = result.Message, data = result.Data });
            return BadRequest(new { message = result.Message });
        }
    }
}

