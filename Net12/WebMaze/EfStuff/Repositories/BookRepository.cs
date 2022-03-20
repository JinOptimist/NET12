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
using WebMaze.Models.Enums;

namespace WebMaze.EfStuff.Repositories
{
    public class BookRepository : BaseRepository<Book>
    {           
        public BookRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<Book> GetAllSortedByParam(string bookFilter, bool asc)
        {
            var table = Expression.Parameter(typeof(Book), "book");

            bookFilter = bookFilter ?? "Author";
            
            var members = new List<MemberExpression>();            
            var newWords = bookFilter.Split('.');

            members.Add(Expression.Property(table, newWords[0]));
            var counter = 1;

            while (counter < newWords.Length)
            {                                
                members.Add(Expression.Property(members[counter - 1], newWords[counter]));                
                counter++;
            } ;

            var condition = Expression.Lambda<Func<Book, string>>(members.Last(), table);

            return (asc ? _dbSet.OrderBy(condition) : _dbSet.OrderByDescending(condition))
                    .ToList();
        }
    }
}