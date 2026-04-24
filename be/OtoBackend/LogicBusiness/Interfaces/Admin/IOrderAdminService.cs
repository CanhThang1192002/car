using LogicBusiness.DTOs;
using System.Threading.Tasks;

namespace LogicBusiness.Interfaces.Admin
{
    public interface IOrderAdminService
    {
        Task<object> GetOrdersForAdminAsync(int page, int pageSize, string? search, string? status, string? paymentStatus, string userRole, int? userShowroomId);
        Task<(bool Success, string Message, object? Data)> CreateOrderAsync(CreateOrderAdminDto dto, string userRole, int? userShowroomId);
    }
}

