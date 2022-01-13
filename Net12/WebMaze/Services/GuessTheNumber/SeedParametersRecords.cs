using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.GuessTheNumberDbModel;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.Services.GuessTheNumber
{
    public class SeedParametersRecords
    {
        public static void SeedGuessTheNumberGameParametersRecords(ModelBuilder modelBuilder)
        {
            var guessTheNumberGameParametersRecords = new[]
            {
                new GuessTheNumberGameParameters
                {
                    Id = 1,
                    Difficulty = GuessTheNumberGameDifficulty.Easy,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberEasy,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberEasy,
                    GameCost = GuessTheNumberGameConstans.GameCostEasy,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameEasy,
                    MaxAttempts = 4,
                    IsActive = true
                },
                new GuessTheNumberGameParameters
                {
                    Id = 2,
                    Difficulty = GuessTheNumberGameDifficulty.Medium,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberMedium,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberMedium,
                    GameCost = GuessTheNumberGameConstans.GameCostMedium,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameMedium,
                    MaxAttempts = 7,
                    IsActive = true
                },
                new GuessTheNumberGameParameters
                {
                    Id = 3,
                    Difficulty = GuessTheNumberGameDifficulty.Hard,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberHard,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberHard,
                    GameCost = GuessTheNumberGameConstans.GameCostHard,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameHard,
                    MaxAttempts = 10,
                    IsActive = true
                }
            };

            modelBuilder.Entity<GuessTheNumberGameParameters>()
                .HasData(guessTheNumberGameParametersRecords);
        }


    }
}
