namespace LogicBusiness.DTOs
{
    public class CreateOrderAdminDto
    {
        public int? UserId { get; set; }
        public string? Phone { get; set; }
        public int CarId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public int? PromotionId { get; set; }
    }
}

