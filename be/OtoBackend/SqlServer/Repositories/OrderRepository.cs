using CoreEntities.Models;
using LogicBusiness.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using SqlServer.DBContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OtoContext _context;

        public OrderRepository(OtoContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetAdminOrdersAsync(
            int page,
            int pageSize,
            string? search,
            string? status,
            string? paymentStatus,
            int? targetShowroomId = null)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .AsQueryable();

            // (Optional) filter by showroom if business later needs it
            // Orders currently link to CarId; showroom filtering would require joining inventories/showroom.
            _ = targetShowroomId;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(o =>
                    (o.OrderCode != null && o.OrderCode.ToLower().Contains(s)) ||
                    (o.UserId != null && o.UserId.ToString()!.Contains(s)) ||
                    (o.CarId != null && o.CarId.ToString()!.Contains(s)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                var st = status.Trim();
                query = query.Where(o => o.Status == st);
            }

            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                var ps = paymentStatus.Trim();
                query = query.Where(o => o.PaymentStatus == ps);
            }

            int totalCount = await query.CountAsync();

            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : System.Math.Min(pageSize, 100);

            query = query.OrderByDescending(o => o.OrderDate ?? System.DateTime.MinValue).ThenByDescending(o => o.OrderId);

            var orders = await query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Include(o => o.Car)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}

