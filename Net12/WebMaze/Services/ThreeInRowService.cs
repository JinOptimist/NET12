﻿using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;
using WebMaze.EfStuff.Repositories.ThreeInRowRepositories;
using AutoMapper;
using WebMaze.Models.ThreeInRow;

namespace WebMaze.Services
{
    public class ThreeInRowService
    {
        private UserService _userService;
        private ThreeInRowCellRepository _threeInRowCellRepository;
        private ThreeInRowGameFieldRepository _threeInRowGameFieldRepository;
        private IMapper _mapper;

        private Random random = new Random();

        public ThreeInRowService(UserService userService,
            ThreeInRowCellRepository threeInRowCellRepository,
            ThreeInRowGameFieldRepository threeInRowGameFieldRepository,
            IMapper mapper)
        {
            _userService = userService;
            _threeInRowCellRepository = threeInRowCellRepository;
            _threeInRowGameFieldRepository = threeInRowGameFieldRepository;
            _mapper = mapper;

        }

        string[] colors = { "none", "red", "green", "blue", "purple", "orange" };

        public ThreeInRowGameField Build()
        {
            var width = 10;
            var height = 10;
            var gameField = new ThreeInRowGameField()
            {
                Width = width,
                Height = height,
                Cells = new List<ThreeInRowCell>(),
                Player = _userService.GetCurrentUser(),
                IsActive = true
            };

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = new ThreeInRowCell()
                    {
                        X = x,
                        Y = y,
                        Color = colors[0],
                        GameField = gameField,
                        IsActive = true
                    };

                    gameField.Cells.Add(cell);
                }
            }

            return gameField;
        }

        public string NextColor()
        {
            return colors[random.Next(1, 6)];
        }

        public void Step(long Id)
        {
            var gameField = _threeInRowGameFieldRepository.Get(2);
            var cell = gameField.Cells.SingleOrDefault(cell => cell.Id == Id);
            cell.Color = NextColor();            

            _threeInRowGameFieldRepository.Save(gameField);

            CheckStatus(Id);
        }

        public void CheckStatus(long Id)
        {
            var gameField = _threeInRowGameFieldRepository.Get(2);
            var gameFieldViewModel = _mapper.Map<ThreeInRowGameFieldViewModel>(gameField);
            var cell = gameFieldViewModel.Cells.SingleOrDefault(cell => cell.Id == Id);

            if (GetNear(gameFieldViewModel, Id) > 2)  
            {
                
            }


            gameField = _mapper.Map<ThreeInRowGameField>(gameFieldViewModel);
            _threeInRowGameFieldRepository.Save(gameField);
        }

        private int GetNear(ThreeInRowGameFieldViewModel gameFieldViewModel, long Id) 
        {
            var cellsSameColor = new List<ThreeInRowCellViewModel>(); 
            for (int x = 0; x < gameFieldViewModel.Width; x++)
            {
                for (int y = 0; y < gameFieldViewModel.Height; y++)
                {
                    var cell = gameFieldViewModel.Cells.SingleOrDefault(c => c.X == x && c.Y == y);
                    var nextCell = gameFieldViewModel.Cells.SingleOrDefault(c => c.X == x && c.Y == y + 1);
                    if (cell.Color == nextCell.Color)
                    {
                        cellsSameColor.Add(cell);
                        cell = nextCell;
                    }
                }
                
            }
            return 1;
        }
    }
} 