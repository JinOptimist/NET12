using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories.SeaBattle
{
    public class SeaBattleDifficultRepository : BaseRepository<SeaBattleDifficult>
    {
        public SeaBattleDifficultRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<SeaBattleDifficult> GetSortedSeaBattles(string columnName = "Height")
           => GetSortedNews(columnName)
           .ToList();

        public List<SeaBattleDifficult> GetFilteredSeaBattles(int min=0, int max=0)
        {
            var table = Expression.Parameter(typeof(SeaBattleDifficult), "seaBattleDifficult");
            var member = Expression.Property(table, "Width");
            var minVal = Expression.Constant(min);
            var maxVal = Expression.Constant(max);

            var eq = Expression.LessThanOrEqual(member, maxVal);// seaBattleDifficult => seaBattleDifficult.Width <=12
            var eq2 = Expression.GreaterThanOrEqual(member, minVal); // seaBattleDifficult => seaBattleDifficult.Width >=10 
            var a = Expression.And(eq, eq2);

            var condition = Expression.Lambda<Func<SeaBattleDifficult, bool>>(a, table);

            return _dbSet
                .Where(condition)
                .ToList();
        }
    }
}
