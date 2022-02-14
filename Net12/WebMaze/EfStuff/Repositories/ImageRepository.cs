using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ImageRepository : BaseRepository<Image>
    {
        public ImageRepository(WebContext webContext) : base(webContext)
        {

        }

        public List<Image> GetSortedBy()
        {
            Expression<Func<Image, bool>> goodImages = x => x.Assessment > 5;

            Expression<Func<Image, int>> dbColumn = x => x.Assessment; // x => x.Assessment
            var compareWith = Expression.Constant(5); // 5
            //var equal = Expression.Equal(dbColumn, compareWith); //x => x.Assessment == 5

            //Expression<Func<Image, bool>> condition = (Expression<Func<Image, bool>>)Expression.Lambda(equal);

            return _dbSet.
                Where(goodImages)
                .ToList();
        }

        public void RemoveByUser(long userId)
        {
            var targetImages = _dbSet.Where(x => x.Author != null && x.Author.Id == userId).ToList();
            targetImages.ForEach(x => Remove(x));
        }

        public Image GetImageByDesc(string desc)
        {
            return _dbSet.SingleOrDefault(x => x.Description == desc);
        }
    }
}
