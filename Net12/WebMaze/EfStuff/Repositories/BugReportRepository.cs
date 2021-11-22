using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class BugReportRepository
    {
        private WebContext _webContext;

        public BugReportRepository(WebContext webContext)
        {
            _webContext = webContext;
        }    

        public List<BugReport> GetAllBugReports()
        {
            return _webContext
                .BugReports
                .Where(x => x.Creater.IsActive)
                .ToList();
        }
        public void dbAddBugReport(BugReport bugReport)
        {            
            _webContext.BugReports.Add(bugReport);
            _webContext.SaveChanges();
        }
    }
}
