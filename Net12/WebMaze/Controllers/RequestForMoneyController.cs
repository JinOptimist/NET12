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
        private TransactionRequestCoins _transactionRequestCoins;

        public RequestForMoneyController(UserRepository userRepository,
            IMapper mapper,
            UserService userService,
            RequestForMoneyRepository requestForMoneyRepository,
            IWebHostEnvironment hostEnvironment,
            TransactionRequestCoins transactionRequestCoins)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _requestForMoneyRepository = requestForMoneyRepository;
            _hostEnvironment = hostEnvironment;
            _transactionRequestCoins = transactionRequestCoins ?? throw new ArgumentException(nameof(transactionRequestCoins));
        }

        public IActionResult RequestCoins()
        {
            var user = _userService.GetCurrentUser();
            var requestViewModel = _requestForMoneyRepository
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
        public IActionResult AddRequestCoins(long requestId)
        {
            var model = _mapper.Map<RequestForMoneyViewModel>(_requestForMoneyRepository.Get(requestId))
         ?? new RequestForMoneyViewModel();           
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddRequestCoins(RequestForMoneyViewModel requestForMoneyViewModel)
        {

            var requestCreator = _userService.GetCurrentUser();
            var requestRecipient = _userRepository.GetUserByName(requestForMoneyViewModel.RequestRecipient);
           
            if (requestRecipient == null)
            {
                ModelState.AddModelError("RequestRecipient", "User with this name not found");
                return View(requestForMoneyViewModel);
            }
            if (requestForMoneyViewModel.RequestAmount == 0)
            {
                ModelState.AddModelError("RequestAmount", "Invalid coin value");
                return View(requestForMoneyViewModel);
            }
            if (requestCreator.Id == requestRecipient.Id)
            {
                ModelState.AddModelError("RequestRecipient", "You are trying to query yourself");
                return View(requestForMoneyViewModel);
            }
            if (requestRecipient.Coins < requestForMoneyViewModel.RequestAmount)
            {
                ModelState.AddModelError("RequestAmount", "The user does not have that many coins");
                return View(requestForMoneyViewModel);
            }
            if (requestForMoneyViewModel.RequestAmount < 0)
            {
                ModelState.AddModelError("RequestAmount", "Invalid coin value");
                return View(requestForMoneyViewModel);
            }
            
            var request = new RequestForMoney
            {
                IsActive = true,
                RequestStatus = RequestStatusEnums.WaitingForAnAnswer,
                RequestCreationDate = DateTime.Now,
                RequestAmount = requestForMoneyViewModel.RequestAmount,
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
            var request = _requestForMoneyRepository.Get(requestId);
            request.RequestStatus = RequestStatusEnums.RequestDenied;
            _requestForMoneyRepository.Save(request);

            return RedirectToAction($"{nameof(RequestForMoneyController.RequestCoins)}");
        }

        [HttpGet]
        public IActionResult AcceptRequestCoins(long requestId)
        {
            var request = _requestForMoneyRepository.Get(requestId);
            var requestCreator = request.RequestCreator;
            var requestRecipient = request.RequestRecipient;           

            if (_transactionRequestCoins.AttemptTransactionRequest(request, requestCreator, requestRecipient))
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

