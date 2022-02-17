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

        public List<Image> GetSortedBy(int assessment, string name)
        {            
            var table = Expression.Parameter(typeof(Image), "image");// image =>            
            //var propAssessment = Expression.Property(table, "Assessment"); // image.Assessment
            //var cond = Expression.Constant(assessment);
            //var greaterThanExpr = Expression.GreaterThanOrEqual(propAssessment, cond);

            //var condition = Expression.Lambda<Func<Image, bool>>(greaterThanExpr, table);// image => image.Assessment >= assessment

            var propAuthor = Expression.Property(table, "Author");
            var propName = Expression.Property(propAuthor, "Name");
            var cond = Expression.Constant(name);
            var equal = Expression.Equal(propName, cond);

            var condition = Expression.Lambda<Func<Image, bool>>(equal, table);// image => image.Assessment >= assessment


            return _dbSet.
                    Where(condition)
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
