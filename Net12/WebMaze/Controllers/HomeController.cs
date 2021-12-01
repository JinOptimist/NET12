using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebContext _webContext;
        private UserService _userService;
        private UserRepository _userRepository;
        private ReviewRepository _reviewRepository;
        private NewCellSuggRepository _newCellSuggRepository;
        private SuggestedEnemysRepository _suggestedEnemysRepository;
        private IMapper _mapper;
        public HomeController(WebContext webContext,
            UserRepository userRepository, ReviewRepository reviewRepository,
            SuggestedEnemysRepository suggestedEnemysRepository,
            IMapper mapper, NewCellSuggRepository newCellSuggRepository, UserService userService)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _suggestedEnemysRepository = suggestedEnemysRepository;
            _mapper = mapper;
            _newCellSuggRepository = newCellSuggRepository;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userViewModels = new List<UserViewModel>();
            foreach (var dbUser in _userRepository.GetAll())
            {
                var userViewModel = new UserViewModel();
                userViewModel.Id = dbUser.Id;
                userViewModel.UserName = dbUser.Name;
                userViewModel.Coins = dbUser.Coins;
                userViewModels.Add(userViewModel);
            }

            //var userViewModels2 = _webContext.Users.Select(
            //    dbModel => new UserViewModel { 
            //        UserName = dbModel.Name, 
            //        Coins = dbModel.Coins 
            //    });

            return View(userViewModels);
        }

        public IActionResult Book()
        {
            var bookViewModels = new List<BookViewModel>();
            foreach (var dbBook in _webContext.Books)
            {
                var bookViewModel = new BookViewModel();
                bookViewModel.Name = dbBook.Name;
                bookViewModel.Link = dbBook.Link;
                bookViewModel.ImageLink = dbBook.ImageLink;
                bookViewModel.Author = dbBook.Author;
                bookViewModel.Desc = dbBook.Desc;
                bookViewModel.ReleaseDate = dbBook.ReleaseDate;
                bookViewModel.PublicationDate = dbBook.PublicationDate;
                bookViewModels.Add(bookViewModel);
            }

            return View(bookViewModels);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBook(BookViewModel bookViewModel)
        {
            var dbBook = new Book()
            {
                Name = bookViewModel.Name,
                Link = bookViewModel.Link,
                ImageLink = bookViewModel.ImageLink,
                Author = bookViewModel.Author,
                Desc = bookViewModel.Desc,
                ReleaseDate = bookViewModel.ReleaseDate,
                PublicationDate = bookViewModel.PublicationDate
            };
            _webContext.Books.Add(dbBook);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userViewMode)
        {
            var dbUser = new User()
            {
                Name = userViewMode.UserName,
                Password = userViewMode.Password,
                Coins = userViewMode.Coins,
                Age = DateTime.Now.Second % 10 + 20,
                IsActive = true
            };

            _userRepository.Save(dbUser);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveUser(long userId)
        {
            _userRepository.Remove(userId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SuggestedEnemys()
        {
            var suggestedEnemysViewModels = new List<SuggestedEnemysViewModel>();
            var suggestedEnemys = _webContext.SuggestedEnemys.ToList();

            suggestedEnemysViewModels = _suggestedEnemysRepository
               .GetAll()
               .Select(dbModel => _mapper.Map<SuggestedEnemysViewModel>(dbModel))
               .ToList();

            return View(suggestedEnemysViewModels);
        }
        [Authorize]
        public IActionResult RemoveSuggestedEnemy(long suggestedEnemysId)
        {
            _suggestedEnemysRepository.Remove(suggestedEnemysId);
            return RedirectToAction($"{nameof(HomeController.SuggestedEnemys)}");
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddSuggestedEnemy()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddSuggestedEnemy(SuggestedEnemysViewModel suggestedEnemysViewModel)
        {
            var creater = _userService.GetCurrentUser();
            //
            var dbSuggestedEnemys = new SuggestedEnemys();
            dbSuggestedEnemys = _mapper.Map<SuggestedEnemys>(suggestedEnemysViewModel);
            dbSuggestedEnemys.IsActive = true;

            _suggestedEnemysRepository.Save(dbSuggestedEnemys);

            return RedirectToAction($"{nameof(HomeController.SuggestedEnemys)}");
        }
        public IActionResult Stuff()
        {
            var staffsForHero = new List<StuffForHeroViewModel>();
            staffsForHero = _staffForHeroRepository
                    .GetAll().Select(dbModel => _mapper.Map<StuffForHeroViewModel>(dbModel)).ToList();
            return View(staffsForHero);
        }

        [HttpGet]
        public IActionResult AddStuffForHero()
        {
            return View();
        }
        public IActionResult AddStuffForHero(StuffForHeroViewModel stuffForHeroViewModel)
        {
            //TODO user current user after login
            var proposer = _userRepository.GetAll()
                .OrderByDescending(x => x.Coins)
                .FirstOrDefault();

            var dbStuffForHero = _mapper.Map<StuffForHero>(stuffForHeroViewModel);

            dbStuffForHero.Proposer = proposer;
            dbStuffForHero.IsActive = true;

            _staffForHeroRepository.Save(dbStuffForHero);
            return RedirectToAction("AddStuffForHero");
        }
        public IActionResult Time()
        {
            var smile = DateTime.Now.Second;
            return View(smile);
        }

        [HttpGet]
        public IActionResult Sum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sum(int x, int y)
        {
            var model = x + y;
            return View(model);
        }


        [HttpGet]
        public IActionResult Reviews()
        {
            var FeedBackUsers = GiveViewReviews();

            return View(FeedBackUsers);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Reviews(FeedBackUserViewModel viewReview)
        {
            if (!ModelState.IsValid)
            {
                var bugFeedBackUsers = GiveViewReviews();
                return View(bugFeedBackUsers);
            }

            var review = _mapper.Map<Review>(viewReview);
            review.Creator = _userService.GetCurrentUser();

            review.IsActive = true;
            _reviewRepository.Save(review);
            var FeedBackUsers = GiveViewReviews();

            return View(FeedBackUsers);
        }

        public IActionResult RemoveReview(long idReview)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var myUser = _userService.GetCurrentUser();
                if (myUser == _reviewRepository.Get(idReview).Creator)
                {
                    _reviewRepository.Remove(idReview);
                }

            }
            return RedirectToAction("Reviews", "Home");
        }
        [HttpGet]
        [Authorize]
        public IActionResult ChangeReviewForm(long idReview)
        {
            var viewReview = _mapper.Map<FeedBackUserViewModel>(_reviewRepository.Get(idReview));
            return View(viewReview);
        }
        [HttpPost]
        public IActionResult ChangeReviewForm(FeedBackUserViewModel review)
        {
            if (!ModelState.IsValid)
            {
                return View(review.Id);
            }

            var DBreview = _userService.GetCurrentUser().MyReviews.SingleOrDefault(rev => rev.Id == review.Id);
            if(DBreview != null)
            {
                DBreview = _mapper.Map<Review>(review);
                _reviewRepository.Save(DBreview);
            }

            return RedirectToAction("Reviews", "Home");
        }

        public List<FeedBackUserViewModel> GiveViewReviews()
        {
            var FeedBackUsers = new List<FeedBackUserViewModel>();
            if (_reviewRepository.GetAll().Any())
            {
                FeedBackUsers = _reviewRepository.GetAll()
                    .Select(rev => _mapper.Map<FeedBackUserViewModel>(rev))
                    .ToList();

                FeedBackUsers = FeedBackUsers
                    .Where(review =>
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            if (review.Creator.Id == _userService.GetCurrentUser().Id)
                            {
                                review.CanEdit = true;
                            }
                            else
                            {
                                review.CanEdit = false;
                            }

                        }
                        else
                        {
                            review.CanEdit = false;
                        }
                        return true;
                    })
                    .ToList();

            }
            return FeedBackUsers;
        }

        public IActionResult NewCellSugg()
        {
            var newCellSuggestionsViewModel = new List<NewCellSuggestionViewModel>();
            newCellSuggestionsViewModel = _newCellSuggRepository.GetAll()
                .Select(dbModel => _mapper.Map<NewCellSuggestionViewModel>(dbModel))
                .ToList();

            return View(newCellSuggestionsViewModel);
        }
        [HttpGet]
        public IActionResult AddNewCellSugg()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewCellSugg(NewCellSuggestionViewModel newCell)
        {
            //TODO user current user after login
            var creater = _userRepository.GetRandomUser();

            var NewCS = new NewCellSuggestion()
            {
                Title = newCell.Title,
                Description = newCell.Description,
                MoneyChange = newCell.MoneyChange,
                HealtsChange = newCell.HealtsChange,
                FatigueChange = newCell.FatigueChange,
                Creater = creater,
                IsActive = true
            };

            _newCellSuggRepository.Save(NewCS);
            return RedirectToAction($"{nameof(HomeController.NewCellSugg)}");
        }
        public IActionResult RemoveNewCellSuggestion(long id)
        {
            _newCellSuggRepository.Remove(id);
            return RedirectToAction($"{nameof(HomeController.NewCellSugg)}");
        }


    }

}
