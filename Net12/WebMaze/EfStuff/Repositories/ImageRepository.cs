using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models.Enums;

namespace WebMaze.EfStuff.Repositories
{
    public class ImageRepository : BaseRepository<Image>
    {
        public ImageRepository(WebContext webContext) : base(webContext)
        {

        }

        public List<Image> GetSorted(object value, string columnName, SortType sortType)
        {
            var table = Expression.Parameter(typeof(Image), "image");
            var propList = columnName.Split(".");
            var prop = Expression.Property(table, propList[0]);

            for (int i = 1; i < propList.Length; i++)
            {
                prop = Expression.Property(prop, propList[i]);
            }

            var condValue = Expression.Constant(null);
            Expression<Func<Image, bool>> condition = null;

            switch (sortType)
            {
                case SortType.Equal:
                    condValue = Expression.Constant(value);
                    var equal = Expression.Equal(prop, condValue);
                    condition = Expression.Lambda<Func<Image, bool>>(equal, table);

                    break;
                case SortType.GreaterThan:
                    condValue = Expression.Constant(Convert.ToInt32(value));
                    var greaterThanExpr = Expression.GreaterThanOrEqual(prop, condValue);
                    condition = Expression.Lambda<Func<Image, bool>>(greaterThanExpr, table);
                    break;
                default:

                    break;
            }

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
