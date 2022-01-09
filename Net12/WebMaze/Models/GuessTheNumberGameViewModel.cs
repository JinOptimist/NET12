﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.Services.GuessTheNumber;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.Models.GuessTheNumber
{
    public class GuessTheNumberGameViewModel
    {
        public long Id { get; set; }
        public DateTime GameDate { get; set; }
        public GuessTheNumberGameStatus GameStatus { get; set; }      
        public int AttemptNumber { get; set; }
        public int GuessedNumber { get; set; }
        public bool IsActive { get; set; }
        public long PlayerId { get; set; }
        public string PlayerName { get;set;}
        public long ParametersId { get; set; }
        public List <GuessTheNumberGameAnswerViewModel> Answers { get; set; }
        public GuessTheNumberGameParametersViewModel Parameters { get; set; }
    }
}
