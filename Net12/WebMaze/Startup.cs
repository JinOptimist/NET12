using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze
{
    public class Startup
    {
        public const string AuthCoockieName = "Smile";
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebMaze12;Integrated Security=True;";
            services.AddDbContext<WebContext>(x => x.UseSqlServer(connectString));

            services.AddAuthentication(AuthCoockieName)
                .AddCookie(AuthCoockieName, config =>
                {
                    config.LoginPath = "/User/Login";
                    config.AccessDeniedPath = "/User/AccessDenied";
                    config.Cookie.Name = "AuthSweet";
                });

            RegisterRepositories(services);

            RegisterMapper(services);

            services.AddScoped<UserService>(x =>
                new UserService(x.GetService<UserRepository>(), x.GetService<IHttpContextAccessor>())
            );

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<UserRepository>(diContainer =>
                {
                    var webContext = diContainer.GetService<WebContext>();
                    var reviewRepository = diContainer.GetService<ReviewRepository>();
                    var imagesRepository = diContainer.GetService<ImageRepository>();
                    var repository = new UserRepository(webContext, reviewRepository, imagesRepository);
                    return repository;
                });

            services.AddScoped<ReviewRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new ReviewRepository(webContext);
                return repository;
            });

            services.AddScoped<StuffRepository>(diContainer =>
                {
                    var webContext = diContainer.GetService<WebContext>();
                    var repository = new StuffRepository(webContext);
                    return repository;
                });

            services.AddScoped<NewsRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new NewsRepository(webContext);
                return repository;
            });

            services.AddScoped<NewCellSuggRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new NewCellSuggRepository(webContext);
                return repository;
            });
            services.AddScoped<SuggestedEnemysRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var suggestedEnemysRepository = new SuggestedEnemysRepository(webContext);
                return suggestedEnemysRepository;
            });

            services.AddScoped<BugReportRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new BugReportRepository(webContext);
                return repository;
            });
            services.AddScoped<MazeLevelRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var userService = diContainer.GetService<UserService>();
                var cellRepository = diContainer.GetService<CellRepository>();
                var mapper = diContainer.GetService<Mapper>();
                var repository = new MazeLevelRepository(webContext, userService, cellRepository, mapper);
                return repository;
            });
            services.AddScoped<CellRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new CellRepository(webContext);
                return repository;
            });

            services.AddScoped<MazeDifficultRepository>(x => new MazeDifficultRepository(x.GetService<WebContext>()));

            services.AddScoped<ImageRepository>();
        }
        private void RegisterMapper(IServiceCollection services)
        {
            var provider = new MapperConfigurationExpression();

            provider.CreateMap<News, NewsViewModel>()
                .ForMember(nameof(NewsViewModel.NameOfAuthor), opt => opt.MapFrom(dbNews => dbNews.Author.Name));
            provider.CreateMap<NewsViewModel, News>();

            provider.CreateMap<SuggestedEnemys, SuggestedEnemysViewModel>()
                    .ForMember(nameof(SuggestedEnemysViewModel.UserName),
                    opt => opt.MapFrom(dbSuggestedEnemys => dbSuggestedEnemys.Creater.Name));
            provider.CreateMap<SuggestedEnemysViewModel, SuggestedEnemys>();


            provider.CreateMap<StuffForHero, StuffForHeroViewModel>();
            provider.CreateMap<StuffForHeroViewModel, StuffForHero>();

            provider.CreateMap<User, UserViewModel>()
                //.ForMember("UserName", opt => opt.MapFrom(x => x.Name))
                .ForMember(nameof(UserViewModel.UserName), opt => opt.MapFrom(dbUser => dbUser.Name))
                .ForMember(nameof(UserViewModel.News), opt => opt.MapFrom(x => x.MyNews));

            provider.CreateMap<Review, FeedBackUserViewModel>()
                .ForMember(nameof(FeedBackUserViewModel.TextInfo), opt => opt.MapFrom(dbreview => dbreview.Text))
                .ForMember(nameof(FeedBackUserViewModel.Creator), opt => opt.MapFrom(dbreview => dbreview.Creator));

            provider.CreateMap<FeedBackUserViewModel, Review>()
                .ForMember(nameof(Review.Text), opt => opt.MapFrom(viewReview => viewReview.TextInfo))
                .ForMember(nameof(Review.Creator), opt => opt.MapFrom(viewReview => viewReview.Creator));

            provider.CreateMap<Image, ImageViewModel>();

            provider.CreateMap<ImageViewModel, Image>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            provider.CreateMap<UserViewModel, User>();

            provider.CreateMap<NewCellSuggestion, NewCellSuggestionViewModel>()
                .ForMember(nameof(NewCellSuggestionViewModel.UserName), opt => opt.MapFrom(dbNewCellSugg => dbNewCellSugg.Creater.Name));
            provider.CreateMap<NewCellSuggestionViewModel, NewCellSuggestion>();
            provider.CreateMap<MazeDifficultProfile, MazeDifficultProfileViewModel>()
                .ForMember(nameof(MazeDifficultProfileViewModel.Author), opt => opt.MapFrom(db => db.Creater.Name));

            provider.CreateMap<MazeDifficultProfileViewModel, MazeDifficultProfile>();

            provider.CreateMap<BugReportViewModel, BugReport>();
            provider.CreateMap<BugReport, BugReportViewModel>();

            provider.CreateMap<MazeLevelModel, MazeViewModel>();
            provider.CreateMap<MazeViewModel, MazeLevelModel>();

            provider.CreateMap<MazeLevelModel, MazeLevelViewId>()
                .ConstructUsing(model => Mazelevel(model))
                .ForMember(maze => maze.MazeLevel.Cells, model => model.MapFrom(mazemodel => mazemodel.Cells))
                .AfterMap((a, b) => b.MazeLevel.Cells.Where(cell =>
                {
                    var baz = cell;
                    baz.Maze = b.MazeLevel;
                    cell = baz;
                    return true;
                }))
                .ForMember(maze => maze.MazeLevel.Height, model => model.MapFrom(mazemod => mazemod.Height))
                .ForMember(maze => maze.MazeLevel.Width, model => model.MapFrom(mazemod => mazemod.Width))
                .ForMember(maze => maze.MazeLevel.Hero, model => model.MapFrom(mazemod => new Hero(mazemod.HeroX, mazemod.HeroY, null, mazemod.HeroNowHp, mazemod.HeroMaxHp)));

            provider.CreateMap<MazeLevelViewId, MazeLevelModel>()
                .ConstructUsing(x => inMazeModel(x))
                .ForMember(mazeview => mazeview.Creator, obd => obd.MapFrom(maze => maze.Creator))
                .AfterMap((a, b) => b.Cells.Where(cell => (cell.MazeLevel = b) == b));

            provider.CreateMap<CellModel, CellViewModelId>()
                .ConstructUsing(x => CellinBaseCell(x));

            provider.CreateMap<CellViewModelId, CellModel>()
                .ConstructUsing(x => BaseCellInCell(x));

            provider.CreateMap<CellModel, CellViewModel>();
            provider.CreateMap<CellViewModel, CellModel>();

            var mapperConfiguration = new MapperConfiguration(provider);

            var mapper = new Mapper(mapperConfiguration);


            services.AddScoped<IMapper>(x => mapper);

        }
        private MazeLevelModel inMazeModel(MazeLevelViewId viewId)
        {
            return new MazeLevelModel()
            {
                Cells = new List<CellModel>(),
                Height = viewId.MazeLevel.Height,
                Width = viewId.MazeLevel.Width,
                HeroMaxFatigure = viewId.MazeLevel.Hero.MaxFatigue,
                HeroMaxHp = viewId.MazeLevel.Hero.Max_hp,
                HeroNowFatigure = viewId.MazeLevel.Hero.CurrentFatigue,
                HeroNowHp = viewId.MazeLevel.Hero.Hp,
                IsActive = true,
                HeroX = viewId.MazeLevel.Hero.X,
                HeroY = viewId.MazeLevel.Hero.Y,
                Id = viewId.Id,
                Name = viewId.Name,

            };
        }
        private MazeLevelViewId Mazelevel(MazeLevelModel model)
        {

            var maze = new MazeLevel
            {
                Height = model.Height,
                Width = model.Width
            };
            maze.Cells = new List<BaseCell>();
            maze.Hero = new Hero(model.HeroX, model.HeroY, maze, model.HeroNowHp, model.HeroMaxHp);

            return new MazeLevelViewId() { Id = model.Id, MazeLevel = maze };
        }
        private CellModel BaseCellInCell(CellViewModelId cell)
        {
            var model = new CellModel
            {
                Id = cell.Id,
                X = cell.Cell.X,
                Y = cell.Cell.Y,
                MazeLevel = null
            };
            if (cell.Cell is Wall)
            {

                model.HpCell = 0;
                model.IsActive = true;
                model.TypeCell = CellInfo.Wall;

            }
            else if (cell.Cell is Ground)
            {
                model.HpCell = 0;
                model.IsActive = true;
                model.TypeCell = CellInfo.Grow;
            }
            else
            {

                model.HpCell = 0;
                model.IsActive = true;
                model.TypeCell = CellInfo.Grow;

            }
            return model;

        }
        private CellViewModelId CellinBaseCell(CellModel model)
        {
            switch (model.TypeCell)
            {
                case CellInfo.Grow:
                    return new CellViewModelId() { Cell = new Ground(model.X, model.Y, null), Id = model.Id };
                case CellInfo.Wall:
                    return new CellViewModelId() { Cell = new Wall(model.X, model.Y, null), Id = model.Id };
                default:
                    return new CellViewModelId() { Cell = new Ground(model.X, model.Y, null), Id = model.Id };
            }

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            //  то €?
            app.UseAuthentication();

            // уда мне можно?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
