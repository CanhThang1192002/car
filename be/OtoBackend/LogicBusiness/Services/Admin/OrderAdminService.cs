using CoreEntities.Models;
using LogicBusiness.DTOs;
using LogicBusiness.Interfaces.Admin;
using LogicBusiness.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LogicBusiness.Services.Admin
{
    public class OrderAdminService : IOrderAdminService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICarRepository _carRepo;
        private readonly IUserRepository _userRepo;

        public OrderAdminService(IOrderRepository orderRepo, ICarRepository carRepo, IUserRepository userRepo)
        {
            _orderRepo = orderRepo;
            _carRepo = carRepo;
            _userRepo = userRepo;
        }

        public async Task<object> GetOrdersForAdminAsync(int page, int pageSize, string? search, string? status, string? paymentStatus, string userRole, int? userShowroomId)
        {
            // Placeholder for potential showroom-based filtering later
            _ = userRole;
            _ = userShowroomId;

            var (orders, total) = await _orderRepo.GetAdminOrdersAsync(page, pageSize, search, status, paymentStatus, null);

            var data = orders.Select(o => new
            {
                o.OrderId,
                o.OrderCode,
                o.UserId,
                UserName = o.User != null ? o.User.FullName : null,
                o.CarId,
                CarName = o.Car != null ? o.Car.Name : null,
                o.OrderDate,
                o.Status,
                o.PaymentStatus,
                o.PaymentMethod,
                o.Subtotal,
                o.DiscountAmount,
                o.FinalAmount,
                o.TotalAmount,
                o.ShippingAddress
            });

            return new
            {
                TotalItems = total,
                CurrentPage = page <= 0 ? 1 : page,
                PageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100),
                TotalPages = (int)Math.Ceiling(total / (double)(pageSize <= 0 ? 10 : Math.Min(pageSize, 100))),
                Data = data
            };
        }

        public async Task<(bool Success, string Message, object? Data)> CreateOrderAsync(CreateOrderAdminDto dto, string userRole, int? userShowroomId)
        {
            _ = userRole;
            _ = userShowroomId;

            if (dto.CarId <= 0) return (false, "CarId không hợp lệ.", null);
            if (dto.Quantity <= 0) return (false, "Quantity không hợp lệ.", null);

            var car = await _carRepo.GetByIdAsync(dto.CarId);
            if (car == null) return (false, "Không tìm thấy xe (CarId) trong hệ thống.", null);

            int? resolvedUserId = dto.UserId;
            if (!resolvedUserId.HasValue && !string.IsNullOrWhiteSpace(dto.Phone))
            {
                var u = await _userRepo.GetUserByPhoneAsync(dto.Phone);
                if (u == null) return (false, "Không tìm thấy người dùng theo SĐT đã nhập.", null);
                resolvedUserId = u.UserId;
            }

            decimal unitPrice = car.Price ?? 0m;
            decimal subtotal = unitPrice * dto.Quantity;
            decimal discount = 0m; // TODO: apply promotion rule if needed
            decimal finalAmount = Math.Max(0m, subtotal - discount);

            var now = DateTime.Now;
            var order = new Order
            {
                UserId = resolvedUserId,
                CarId = dto.CarId,
                OrderDate = now,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Pending" : dto.Status.Trim(),
                PaymentStatus = string.IsNullOrWhiteSpace(dto.PaymentStatus) ? "Unpaid" : dto.PaymentStatus.Trim(),
                PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? null : dto.PaymentMethod.Trim(),
                ShippingAddress = string.IsNullOrWhiteSpace(dto.ShippingAddress) ? null : dto.ShippingAddress.Trim(),
                PromotionId = dto.PromotionId,
                OrderCode = $"OD{now:yyyyMMddHHmmss}{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
                Subtotal = subtotal,
                DiscountAmount = discount,
                FinalAmount = finalAmount,
                TotalAmount = finalAmount
            };

            // Minimal order item
            order.OrderItems.Add(new OrderItem
            {
                CarId = dto.CarId,
                Quantity = dto.Quantity,
                Price = unitPrice
            });

            await _orderRepo.AddAsync(order);

            return (true, "Tạo đơn hàng thành công.", new { order.OrderId, order.OrderCode });
        }
    }
}

