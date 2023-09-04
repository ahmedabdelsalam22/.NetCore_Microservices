namespace Mango.Services.CouponAPI.Models.DTOS
{
    public class CouponCreateDTO
    {
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
