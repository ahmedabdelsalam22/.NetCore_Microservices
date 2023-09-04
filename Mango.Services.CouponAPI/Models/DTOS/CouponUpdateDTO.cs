namespace Mango.Services.CouponAPI.Models.DTOS
{
    public class CouponUpdateDTO
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
