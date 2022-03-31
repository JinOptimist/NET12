using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff
{
    public static class IQueryableExtension
    {

        public static IQueryable<Template> GetQueryableForPagination<Template>(this IQueryable<Template> templates, int perPage, int page)
            => templates
            .Skip((page - 1) * perPage)
            .Take(perPage);
    }
}
