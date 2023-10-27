using Mango.Services.RewardAPI.Data;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mango.Services.RewardAPI.Service
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public RewardService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            this._dbOptions = dbOptions;
        }

        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            Rewards rewards = new() 
            {
                OrderId = rewardsMessage.OrderId,
                UserId = rewardsMessage.UserId,
                RewardsActivity = rewardsMessage.RewardsActivity,
                RewardsDate = DateTime.Now
            };

            await using var _db = new ApplicationDbContext(_dbOptions);
            await _db.Rewards.AddAsync(rewards);
            await _db.SaveChangesAsync();
        }

    }
}
