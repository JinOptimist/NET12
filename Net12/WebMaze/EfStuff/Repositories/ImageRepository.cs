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

        public List<Image> GetSorted (object value, string columnName = "Assesment")
        {
            var table = Expression.Parameter(typeof(Image), "image");// image =>            

            var propAssessment = Expression.Property(table, columnName); // image.Assessment
            var cond = Expression.Constant(value);
            var greaterThanExpr = Expression.GreaterThanOrEqual(propAssessment, cond);

            var condition = Expression.Lambda<Func<Image, bool>>(greaterThanExpr, table);// image => image.Assessment >= assessment


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
