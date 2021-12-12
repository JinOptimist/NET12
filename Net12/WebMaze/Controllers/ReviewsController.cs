using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{

    public class ReviewsController : Controller
    {
        private ReviewRepository _reviewRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;
        public ReviewsController(IMapper mapper, UserService userService, ReviewRepository reviewRepository, 
            PayForActionService payForActionService)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var FeedBackUsers = _reviewRepository.GiveViewReviews(_userService);

            return View(FeedBackUsers);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(FeedBackUserViewModel viewReview)
        {
            if (!ModelState.IsValid)
            {
                var bugFeedBackUsers = _reviewRepository.GiveViewReviews(_userService);
                return View(bugFeedBackUsers);
            }

            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to review");
                return View(viewReview);
            }

            var review = _mapper.Map<Review>(viewReview);
            review.Creator = _userService.GetCurrentUser();

            review.IsActive = true;
            _reviewRepository.Save(review);
            var FeedBackUsers = _reviewRepository.GiveViewReviews(_userService);

            return View(FeedBackUsers);
        }

        public IActionResult Wonderful(long reviewId)
        {
            var review = _reviewRepository.Get(reviewId);
            _payForActionService.EarnMoney(review.Creator.Id, 10);

            return RedirectToAction("Index", "Reviews");
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
            return RedirectToAction("Index", "Reviews");
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
            if (DBreview != null)
            {
                var rev = _mapper.Map<Review>(review);
                DBreview.Text = rev.Text;
                DBreview.Rate = rev.Rate;
                _reviewRepository.Save(DBreview);
            }

            return RedirectToAction("Index", "Reviews");
        }


    }
}
