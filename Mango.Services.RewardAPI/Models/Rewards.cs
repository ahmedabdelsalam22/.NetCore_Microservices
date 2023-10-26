namespace Mango.Services.RewardAPI.Models
{
    public class Rewards
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime RewardsActivity { get; set; }
        public int OrderId { get; set; }
    }
}
