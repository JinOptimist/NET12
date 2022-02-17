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

        public List<Image> GetSortedBy(string prop = "Assessment")
        {
            //split column name by . then foreach expr.prop
            var table = Expression.Parameter(typeof(Image), "image");// image =>
            var allProp = prop.Split(".");
            foreach (var item in allProp)
            {

            }
            var author = Expression.Property(table, "Author"); // image.Assessment
            var authorName = Expression.Property(author, "Name"); // image.Assessment
            var cond = Expression.Constant(name);
            //var greaterThanExpr = Expression.GreaterThanOrEqual(member, cond);
            var eq = Expression.Equal(authorName, cond);// news => news.Title == 'good news'


            var condition = Expression.Lambda<Func<Image, bool>>(eq, table);

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
