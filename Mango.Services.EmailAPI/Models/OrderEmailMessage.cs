namespace Mango.Services.EmailAPI.Models
{
    public class OrderEmailMessage
    {
        public string UserId { get; set; }
        public int OrderId { get; set; }
        public int RewardsActivity { get; set; }
        public DateTime RewardsDate { get; set; }
    }
}
