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
            var counter = 0;

            if (newWords[0] == "OldDate" || newWords[0] == "NewDate")
            {
                newWords[0] = "ReleaseDate";
            }

            do
            {                
                if (counter == 0)
                {
                    members.Add(Expression.Property(table, newWords[counter]));
                }
                else
                {
                    members.Add(Expression.Property(members[counter - 1], newWords[counter]));
                }
                counter++;
            } while (counter < newWords.Length) ;

            var condition = Expression.Lambda<Func<Book, string>>(members[counter - 1], table);

            return (asc ? _dbSet.OrderBy(condition) : _dbSet.OrderByDescending(condition))
                    .ToList();

            /*bookFilter = bookFilter ?? BookFilter.Author;
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
                    .ToList();*/
        }

    }
}