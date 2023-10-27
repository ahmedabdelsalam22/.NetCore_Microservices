
using Mango.Services.RewardAPI.Models;

namespace Mango.Services.RewardAPI.Service
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
