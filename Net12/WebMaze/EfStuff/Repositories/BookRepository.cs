using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace WebMaze.EfStuff.Repositories
{
    public class BookRepository : BaseRepository<Book>
    {           
        public BookRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<Book> GetAllSortedByAuthor()
        {
            var table = Expression.Parameter(typeof(Book), "book");// book =>
            var member = Expression.Property(table, "Author"); // book.Author

            var condition = Expression.Lambda<Func<Book, string>>(member, table);

            return _dbSet
                .OrderBy(condition)// book => book.Author
                /*.OrderBy(x => x.Author)*/
                .ToList();
        }

    }
}