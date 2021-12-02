using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.EfStuff.Repositories
{
    public class ReviewRepository : BaseRepository<Review>
    {
        private IMapper _mapper;

        public ReviewRepository(WebContext webContext, IMapper mapper) : base(webContext)
        {
            _mapper = mapper;

        }
        public List<FeedBackUserViewModel> GiveViewReviews(UserService _userService)
        {
            var FeedBackUsers = new List<FeedBackUserViewModel>();
            if (GetAll().Any())
            {
                FeedBackUsers = GetAll()
                    .Select(rev => _mapper.Map<FeedBackUserViewModel>(rev))
                    .ToList();

                FeedBackUsers = FeedBackUsers
                    .Where(review =>
                    {
                        if (_userService.GetCurrentUser() != null)
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
    }
}
