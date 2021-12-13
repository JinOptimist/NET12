using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class NewsCommentController : Controller
    {
        private NewsCommentRepository _newsCommentRepository;
        private UserRepository _userRepository;
        private NewsRepository _newsRepository;
        private IMapper _mapper;
        private UserService _userService;

        public NewsCommentController(UserRepository userRepository,
            NewsRepository newsRepository,
            IMapper mapper,
            UserService userService,
            NewsCommentRepository newsCommentRepository)
        {
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _newsCommentRepository = newsCommentRepository;
        }

        public IActionResult Index(long newsId)
        {
            var result = new NewsAndCommentsViewModel();
            var news = _mapper.Map<NewsViewModel>(_newsRepository.Get(newsId));
            var newsCommentsViewModels = new List<NewsCommentViewModel>();
            newsCommentsViewModels = _newsCommentRepository
                .GetAllId(newsId)
                .Select(dbModel => _mapper.Map<NewsCommentViewModel>(dbModel))
                .ToList();

            result.News = news;
            result.NewsComments = newsCommentsViewModels;

            return View(result);
        }

        [HttpGet]
        public IActionResult AddNewsComment(long commentId, long newsId)
        {
            var model = _mapper.Map<NewsCommentViewModel>(_newsCommentRepository.Get(commentId))
                ?? new NewsCommentViewModel() { NewsId = newsId };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddNewsComment(NewsCommentViewModel newsCommentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newsCommentViewModel);
            }

            var author = _userService.GetCurrentUser();
            var dbNewsComments = _mapper.Map<NewsComment>(newsCommentViewModel);
            var news = _newsRepository.Get(newsCommentViewModel.NewsId);

            dbNewsComments.CreationDate = DateTime.Now.Date;
            dbNewsComments.Author = author;
            dbNewsComments.IsActive = true;
            dbNewsComments.News = news;

            _newsCommentRepository.Save(dbNewsComments);

            return RedirectToAction("Index", "NewsComment", new { newsCommentViewModel.NewsId });
        }

        //public IActionResult RemoveNews(long newsId)
        //{
        //    _newsRepository.Remove(newsId);
        //    return RedirectToAction("Index", "News");
        //}
    }
}