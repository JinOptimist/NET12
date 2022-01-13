using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebMaze.EfStuff.DbModel.GuessTheNumberDbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.GuessTheNumber;
using WebMaze.Models.GuessTheNumber;
using WebMaze.Services;
using WebMaze.Services.GuessTheNumber;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.Controllers
{
    public class GuessTheNumberController : Controller
    {
        private UserRepository _userRepository;
        private IMapper _mapper;
        private UserService _userService;
        private GuessTheNumberGameRepository _guessTheNumberGameRepository;
        private GuessTheNumberGameAnswerRepository _guessTheNumberGameAnswerRepository;
        private GuessTheNumberGameParametersRepository _guessTheNumberGameParametersRepository;

        public GuessTheNumberController(
            UserRepository userRepository,
            GuessTheNumberGameRepository guessTheNumberGameRepository,
            GuessTheNumberGameAnswerRepository guessTheNumberGameAnswerRepository,
            GuessTheNumberGameParametersRepository guessTheNumberGameParametersRepository,
            IMapper mapper,
            UserService userService)
        {
            _guessTheNumberGameRepository = guessTheNumberGameRepository;
            _guessTheNumberGameParametersRepository = guessTheNumberGameParametersRepository;
            _guessTheNumberGameAnswerRepository = guessTheNumberGameAnswerRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;

        }

        public IActionResult CheckForUnfinishedGame()
        {

            var player = _userService.GetCurrentUser();
            var game = player.GuessTheNumberGames
                .SingleOrDefault(x => x.GameStatus == GuessTheNumberGameStatus.NotFinished);          
            if (game == null)
            {
                return RedirectToAction($"{nameof(GuessTheNumberController.StartGame)}");
            }

            return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberGameAnswer)}",
                new { gameId = game.Id });
        }

        [HttpGet]
        public IActionResult StartGame()
        {

            return View();
        }

        [HttpPost]
        public IActionResult StartGame(GuessTheNumberGameDifficulty difficulty)
        {
            var player = _userService.GetCurrentUser();

            var gameParameters = _guessTheNumberGameParametersRepository
                .GetAll()
                .Single(p => p.Difficulty == difficulty);

            Random rnd = new Random();
            int value = rnd.Next(gameParameters.MinRangeNumber, gameParameters.MaxRangeNumber + 1);
            var hiddenNumber = value;

            var game = new GuessTheNumberGame
            {
                ParametersId = gameParameters.Id,
                AttemptNumber = gameParameters.MaxAttempts,
                GameDate = DateTime.Now,
                GameStatus = GuessTheNumberGameStatus.NotFinished,
                GuessedNumber = hiddenNumber,
                PlayerId = player.Id,
                IsActive = true
            };

            player.Coins = player.Coins - gameParameters.GameCost;
            _userRepository.Save(player);
            _guessTheNumberGameRepository.Save(game);

            return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberGameAnswer)}", new { gameId = game.Id });
        }

        public IActionResult GuessTheNumberGamePlay(long gameId)
        {
            var guessTheNumberGameAnswerViewModels = new List<GuessTheNumberGameAnswerViewModel>();
            guessTheNumberGameAnswerViewModels = _guessTheNumberGameAnswerRepository
                .GetAll()
                .Select(dbModel => _mapper.Map<GuessTheNumberGameAnswerViewModel>(dbModel))
                .ToList();

            return View(guessTheNumberGameAnswerViewModels);
        }

        [HttpGet]
        public IActionResult GuessTheNumberGameAnswer(long gameId)
        {
            var game = _guessTheNumberGameRepository.Get(gameId);
            var gameModel = _mapper.Map<GuessTheNumberGameViewModel>(game);

            var model = new GuessTheNumberGameAnswerViewModel
            {
                Game = gameModel,
                GameId = gameId
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult GuessTheNumberGameAnswer(
            GuessTheNumberGameAnswerViewModel answerModel)
        {

            var user = _userService.GetCurrentUser();
            var game = _guessTheNumberGameRepository.Get(answerModel.GameId);

            if (game.GameStatus == GuessTheNumberGameStatus.NotFinished)
            {
                game.AttemptNumber--;
                var answer = _mapper.Map<GuessTheNumberGameAnswer>(answerModel);
                answer.IsActive = true;
                _guessTheNumberGameAnswerRepository.Save(answer);

                if (answerModel.IntroducedAnswer == game.GuessedNumber)
                {
                    game.GameStatus = GuessTheNumberGameStatus.Win;
                    user.Coins = user.Coins + game.Parameters.RewardForWinningTheGame;
                    _userRepository.Save(user);
                }
                else if (game.AttemptNumber <= 0)
                {
                    game.GameStatus = GuessTheNumberGameStatus.Loss;
                }

                _guessTheNumberGameRepository.Save(game);
            }            

            return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberGameAnswer)}", new { gameId = game.Id });
        }

        [HttpGet]
        public IActionResult IGiveUp()
        {
            var user = _userService.GetCurrentUser();
            var game = _guessTheNumberGameRepository
                .GetAll()
                .Single(g =>
                    g.GameStatus == GuessTheNumberGameStatus.NotFinished
                    &&
                    g.PlayerId == user.Id);

            game.GameStatus = GuessTheNumberGameStatus.Loss;
            _guessTheNumberGameRepository.Save(game);

            return RedirectToAction("Index", "Home");
        }

    }
}
