using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;
using WebMaze.Services.RequestForMoney;

namespace WebMaze.Controllers
{
    public class RequestForMoneyController : Controller
    {

        private UserRepository _userRepository;
        private IMapper _mapper;
        private UserService _userService;
        private RequestForMoneyRepository _requestForMoneyRepository;
        private IWebHostEnvironment _hostEnvironment;
        public RequestForMoneyController(UserRepository userRepository,
            IMapper mapper,
            UserService userService,
            RequestForMoneyRepository requestForMoneyRepository,
            IWebHostEnvironment hostEnvironment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _requestForMoneyRepository = requestForMoneyRepository;
            _hostEnvironment = hostEnvironment;

        }

        public IActionResult RequestCoins()
        {
            var user = _userService.GetCurrentUser();
            var requestViewModel = new List<RequestForMoneyViewModel>();
            requestViewModel = _requestForMoneyRepository
               .GetAllRequestsCurrentUser(user.Id)
               .Select(dbModel => _mapper.Map<RequestForMoneyViewModel>(dbModel))
               .ToList();
            foreach (var item in requestViewModel)
            {
                if (item.RequestAmount > user.Coins && item.RequestStatus == RequestStatusEnums.WaitingForAnAnswer)
                {
                    item.MassegeErrors = MassegeErrorsRequestEnums.NotEnoughCoins;
                }
            }
            return View(requestViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddRequestCoins(long requestId, MassegeErrorsRequestEnums massege)
        {
            var model = _mapper.Map<RequestForMoneyViewModel>(_requestForMoneyRepository.Get(requestId))
         ?? new RequestForMoneyViewModel();
            model.MassegeErrors = massege;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddRequestCoins(string userName, int requestCoins)
        {

            var requestCreator = _userService.GetCurrentUser();
            var requestRecipient = _userRepository.GetUserByName(userName);
            if (requestRecipient == null)
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.AddRequestCoins)}", new { massege = MassegeErrorsRequestEnums.NotEnoughUser });
            }
            if (requestCoins == 0)
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.AddRequestCoins)}", new { massege = MassegeErrorsRequestEnums.NegativeValue });
            }
            if (requestCreator.Id == requestRecipient.Id)
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.AddRequestCoins)}", new { massege = MassegeErrorsRequestEnums.RequestToYourself });
            }
            if (requestRecipient.Coins < requestCoins)
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.AddRequestCoins)}", new { massege = MassegeErrorsRequestEnums.UserNotEnoughCoins });
            }
            if (requestCoins < 0)
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.AddRequestCoins)}", new { massege = MassegeErrorsRequestEnums.NegativeValue });
            }

            var request = new RequestForMoney
            {
                IsActive = true,
                RequestStatus = RequestStatusEnums.WaitingForAnAnswer,
                RequestCreationDate = DateTime.Now,
                RequestAmount = requestCoins,
                RequestCreator = requestCreator,
                RequestRecipient = requestRecipient
            };
            _requestForMoneyRepository.Save(request);

            return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoins)}");
        }


        public IActionResult RemoveRequestCoins(long requestId)
        {
            _requestForMoneyRepository.Remove(requestId);
            return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoins)}");
        }


        [HttpGet]
        public IActionResult RejectRequestCoins(long requestId)
        {
            var requestRecipient = _userService.GetCurrentUser();
            var request = _requestForMoneyRepository.Get(requestId);
            request.RequestStatus = RequestStatusEnums.RequestDenied;
            _requestForMoneyRepository.Save(request);

            return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoins)}");
        }


        [HttpGet]
        public IActionResult AcceptRequestCoins(long requestId)
        {
            var requestRecipient = _userService.GetCurrentUser();
            var request = _requestForMoneyRepository.Get(requestId);
            var requestCreator = _userRepository.Get(request.RequestCreator.Id);
            if (_requestForMoneyRepository.TrasactionRequest(request, requestCreator, requestRecipient))
            {
                return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoins)}");
            }
            return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoinsError)}");
        }

        public IActionResult RequestCoinsError()
        {
            return View();
        }
    }
}

