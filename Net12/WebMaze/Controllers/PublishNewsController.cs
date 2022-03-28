using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class PublishNewsController : Controller
    {
        private NewsRepository _newsRepository;

        public PublishNewsController(NewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public IActionResult PublishNews()
        {
            while (true)
            {
                Thread thread = new Thread(PublishNews);
                thread.Start();

                while (thread.ThreadState.ToString() != "Stopped")
                    Thread.Sleep(60000);
            }

            void PublishNews()
            {
                var connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebMaze12;Integrated Security=True;";
                var optionBuilder = new DbContextOptionsBuilder<WebContext>();
                var options = optionBuilder.UseSqlServer(connectString).Options;
                using (WebContext db = new WebContext(options))
                {
                    var records = db.News.Where(x => x.IsActive && x.DateTimeOfPublication <= DateTime.Today && x.IsPublished == false);
                    foreach (var news in records)
                    {
                        news.IsPublished = true;
                        db.Update(news);
                    }
                    db.SaveChanges();
                }
            }
            return View();
        }
    }
}
