using CoreEntities.Models;
using LogicBusiness.DTOs;
using LogicBusiness.Interfaces.Admin;
using LogicBusiness.Interfaces.Repositories;

namespace LogicBusiness.Services.Admin
{
    public class BannerAdminService : IBannerAdminService
    {
        private readonly IBannerRepository _repo;

        public BannerAdminService(IBannerRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Banner>> GetAllAsync(bool? isActive = null)
        {
            return _repo.GetAllAsync(isActive);
        }

        public Task<Banner?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Banner? Data)> CreateAsync(BannerCreateDto dto)
        {
            if (dto.EndDate.HasValue && dto.StartDate.HasValue && dto.EndDate.Value < dto.StartDate.Value)
            {
                return (false, "EndDate không được nhỏ hơn StartDate.", null);
            }

            var banner = new Banner
            {
                BannerName = dto.BannerName.Trim(),
                ImageUrl = dto.ImageUrl.Trim(),
                LinkUrl = string.IsNullOrWhiteSpace(dto.LinkUrl) ? null : dto.LinkUrl.Trim(),
                Position = dto.Position,
                IsActive = dto.IsActive,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _repo.AddAsync(banner);
            return (true, "Tạo banner thành công!", banner);
        }

        public async Task<(bool Success, string Message, Banner? Data)> UpdateAsync(int id, BannerUpdateDto dto)
        {
            if (dto.EndDate.HasValue && dto.StartDate.HasValue && dto.EndDate.Value < dto.StartDate.Value)
            {
                return (false, "EndDate không được nhỏ hơn StartDate.", null);
            }

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return (false, "Không tìm thấy banner!", null);

            existing.BannerName = dto.BannerName.Trim();
            existing.ImageUrl = dto.ImageUrl.Trim();
            existing.LinkUrl = string.IsNullOrWhiteSpace(dto.LinkUrl) ? null : dto.LinkUrl.Trim();
            existing.Position = dto.Position;
            existing.IsActive = dto.IsActive;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;

            await _repo.UpdateAsync(existing);
            return (true, "Cập nhật banner thành công!", existing);
        }
    }
}

