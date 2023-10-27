namespace Mango.Services.RewardAPI.Models
{
    public class RewardsMessage
    {
        public string UserId { get; set; }
        public int OrderId { get; set; }
        public int RewardsActivity { get; set; }
        public DateTime RewardsDate { get; set; }
    }
}
