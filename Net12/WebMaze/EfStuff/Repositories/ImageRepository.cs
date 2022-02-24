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

            ConstantExpression conditionValue;
            BinaryExpression operation;
            Expression<Func<Image, bool>> condition;

            switch (sortType)
            {
                case SortType.Equal:
                    conditionValue = Expression.Constant(value);
                    operation = Expression.Equal(prop, conditionValue);
                    break;
                case SortType.GreaterThan:
                    conditionValue = Expression.Constant(Convert.ToInt32(value));
                    operation = Expression.GreaterThanOrEqual(prop, conditionValue);
                    break;
                default:
                    throw new Exception();
                    break;
            }
            condition = Expression.Lambda<Func<Image, bool>>(operation, table);

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
