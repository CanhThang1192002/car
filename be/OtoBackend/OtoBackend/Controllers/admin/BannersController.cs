using LogicBusiness.DTOs;
using LogicBusiness.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogicBusiness.Utilities;

namespace OtoBackend.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, ShowroomManager, SalesManager")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerAdminService _service;

        public BannersController(IBannerAdminService service)
        {
            _service = service;
        }

        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string? bannerName = null)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "Vui lòng chọn file ảnh." });

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var allowed = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
            if (string.IsNullOrEmpty(ext) || !allowed.Contains(ext))
            {
                return BadRequest(new { message = "Chỉ hỗ trợ ảnh .jpg, .jpeg, .png, .webp" });
            }

            var target = string.IsNullOrWhiteSpace(bannerName) ? "banner" : bannerName!;
            var imageUrl = await FileHelper.UploadFileAsync(file, "Banners", target, customFileName: $"banner-{Guid.NewGuid():N}");
            return Ok(new { message = "Upload ảnh banner thành công!", imageUrl });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null)
        {
            var data = await _service.GetAllAsync(isActive);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var banner = await _service.GetByIdAsync(id);
            if (banner == null) return NotFound(new { message = "Không tìm thấy banner!" });
            return Ok(banner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BannerCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BannerUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}

