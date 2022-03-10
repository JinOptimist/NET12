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

        public List<Book> GetAllSortedByParam(BookFilter? bookFilter, bool asc)
        {         
            bookFilter = bookFilter ?? BookFilter.Author;     
            if (bookFilter == BookFilter.OldDate || bookFilter == BookFilter.NewDate)
                bookFilter = BookFilter.ReleaseDate;
            string stringBookFilter = bookFilter.ToString();

            var typeCreator = (bookFilter == BookFilter.Creator) ? true : false;

            var table = Expression.Parameter(typeof(Book), "book");// book =>

            MemberExpression member;

            if (typeCreator)
            {
                var field = "Name";
                var middleMember = Expression.Property(table, stringBookFilter);
                member = Expression.Property(middleMember, field); // book.Creator.Name
            }
            else
            {
                member = Expression.Property(table, stringBookFilter); // book.Author
            };

                var condition = Expression.Lambda<Func<Book, string>>(member, table);

                return (asc ? _dbSet.OrderBy(condition) : _dbSet.OrderByDescending(condition))                   
                    .ToList();
        }

    }
}