using System;
using System.ComponentModel.DataAnnotations;

namespace LogicBusiness.DTOs
{
    public class BannerCreateDto
    {
        [Required]
        public string BannerName { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        public string? LinkUrl { get; set; }

        public int Position { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class BannerUpdateDto
    {
        [Required]
        public string BannerName { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        public string? LinkUrl { get; set; }

        public int Position { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}

