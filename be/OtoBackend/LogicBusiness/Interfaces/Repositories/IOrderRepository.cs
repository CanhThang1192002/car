using CoreEntities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicBusiness.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetAdminOrdersAsync(
            int page,
            int pageSize,
            string? search,
            string? status,
            string? paymentStatus,
            int? targetShowroomId = null);

        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int orderId);
    }
}

