using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewsRepository : BaseRepository<News>
    {
        public NewsRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<News> GetAllSorted()
        {
            var table = Expression.Parameter(typeof(News), "news");// news =>
            var member = Expression.Property(table, "Title"); // news.Title
            var constName = Expression.Constant("Good news"); // 'good news'
            var eq = Expression.Equal(member, constName);// news => news.Title == 'good news'

            var condition = Expression.Lambda<Func<News, bool>>(eq, table);

            return _dbSet
                .Where(condition)
                .ToList();
        }

        public News GetNewsByName(string title)
        {
            return _dbSet.SingleOrDefault(x => x.Title == title);
        }


        public List<News> GetForPagination(int perPage, int page, string columnName = "CreationDate")
            => GetSortedNews(columnName)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();


        public List<News> GetFiltList(int TypeOfSearch, string columnName, string TextSearch)
        {


            List<News> MyList;
            var table = Expression.Parameter(typeof(News), "news");// news =>

            var ListOfProperty = columnName.Split(".");
            var member = Expression.Property(table, ListOfProperty[0]);
            for (int i = 1; i < ListOfProperty.Length; i++)
            {
                var item = ListOfProperty[i];
                var next = Expression.Property(member, item);
                member = next;

            }
            
            var constName = Expression.Constant(TextSearch, typeof(string)); // 'good news'
            if (TypeOfSearch == 1)
            {
     

                var eq = Expression.Equal(Expression.Convert(Expression.Convert(member,typeof(object)),typeof(string)), constName);// news => news.Title == 'good news'
                var condition = Expression.Lambda<Func<News, bool>>(eq, table);

                MyList = _dbSet.Where(condition).ToList();
            } else if(TypeOfSearch == 2)
            {

                
                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var memberInObj = Expression.Convert(member, typeof(object));
                var memberInString = Expression.Convert(memberInObj, typeof(string));
                var containsMethodExp = Expression.Call(memberInString, method, constName);

                
                var condition = Expression.Lambda<Func<News, bool>>(containsMethodExp, table);

                MyList = _dbSet.Where(condition).ToList();
            } else
            {
                MyList = new List<News>();
            }

            return MyList;
        }
    }


}
