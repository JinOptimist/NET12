using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebMaze.EfStuff.DbModel.GuessTheNumber;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.GuessTheNumber;
using WebMaze.Models.GuessTheNumber;
using WebMaze.Services;
using WebMaze.Services.GuessTheNumber;

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
                .Get(difficulty);

            if (gameParameters is null)
            {
                return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberError)}");
            }

            Random rnd = new Random();
            int value = rnd.Next(gameParameters.MinRangeNumber, gameParameters.MaxRangeNumber + 1);
            var hiddenNumber = value;

            var game = new GuessTheNumberGame
            {
                Player = player,
                Parameters = gameParameters,
                AttemptNumber = gameParameters.MaxAttempts,
                StartDateGame = DateTime.Now,
                GameStatus = GuessTheNumberGameStatus.NotFinished,
                GuessedNumber = hiddenNumber,
                IsActive = true
            };

            player.Coins = player.Coins - gameParameters.GameCost;
            _userRepository.Save(player);
            _guessTheNumberGameRepository.Save(game);

            return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberGameAnswer)}", new { gameId = game.Id });
        }

        [HttpGet]
        public IActionResult GuessTheNumberGameAnswer(long gameId)
        {
            var game = _guessTheNumberGameRepository.Get(gameId);
            var gameModel = _mapper.Map<GuessTheNumberGameViewModel>(game);
            var model = new GuessTheNumberGameAnswerViewModel
            {
                Game = gameModel
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
                answer.Game = game;
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
                .GetCurrentGame(user.Id);

            if (game is null)
            {
                return RedirectToAction($"{nameof(GuessTheNumberController.GuessTheNumberError)}");
            }

            game.GameStatus = GuessTheNumberGameStatus.Loss;
            _guessTheNumberGameRepository.Save(game);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult GuessTheNumberError()
        {
            return View();
        }
    }
}