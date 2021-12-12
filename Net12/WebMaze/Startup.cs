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
                    var mazeLevelRepository = diContainer.GetService<MazeLevelRepository>();
                    var cellRepository = diContainer.GetService<CellRepository>();
                    var imagesRepository = diContainer.GetService<ImageRepository>();
                    var favGamesRepository = diContainer.GetService<FavGamesRepository>();
                    var repository = new UserRepository(webContext, reviewRepository, imagesRepository, mazeLevelRepository, cellRepository, favGamesRepository);
                    return repository;
                });

            services.AddScoped<ReviewRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var mapper = diContainer.GetService<IMapper>();
                var repository = new ReviewRepository(webContext, mapper);
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
                var repository = new MazeLevelRepository(webContext);
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

            services.AddScoped<PayForActionService>();

            services.AddScoped<FavGamesRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new FavGamesRepository(webContext);
                return repository;
            });

            services.AddScoped<MinerCellRepository>();

            services.AddScoped<MinerFieldRepository>();

            services.AddScoped<MinerFiledBuilder>();

            services.AddScoped<PermissionRepository>();
            
        }
        private void RegisterMapper(IServiceCollection services)
        {
            var provider = new MapperConfigurationExpression();

            provider.CreateMap<News, NewsViewModel>()
                .ForMember(nameof(NewsViewModel.NameOfAuthor), opt => opt.MapFrom(dbNews => dbNews.Author.Name))
                .ForMember(nameof(NewsViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Author.GlobalUserRating));
            provider.CreateMap<NewsViewModel, News>();

            provider.CreateMap<SuggestedEnemys, SuggestedEnemysViewModel>()
                    .ForMember(nameof(SuggestedEnemysViewModel.UserName), opt => opt.MapFrom(dbSuggestedEnemys => dbSuggestedEnemys.Creater.Name))
                    .ForMember(nameof(SuggestedEnemysViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));
            provider.CreateMap<SuggestedEnemysViewModel, SuggestedEnemys>();


            provider.CreateMap<StuffForHero, StuffForHeroViewModel>()
                .ForMember(nameof(StuffForHeroViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Proposer.GlobalUserRating))
                .ForMember(nameof(StuffForHeroViewModel.Proposer), opt => opt.MapFrom(dbStuff => dbStuff.Proposer.Name));
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

            provider.CreateMap<Image, ImageViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));

            provider.CreateMap<ImageViewModel, Image>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            provider.CreateMap<UserViewModel, User>();

            provider.CreateMap<NewCellSuggestion, NewCellSuggestionViewModel>()
                .ForMember(nameof(NewCellSuggestionViewModel.UserName), opt => opt.MapFrom(dbNewCellSugg => dbNewCellSugg.Creater.Name))
                .ForMember(nameof(NewCellSuggestionViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));
            provider.CreateMap<NewCellSuggestionViewModel, NewCellSuggestion>();

            provider.CreateMap<MazeDifficultProfile, MazeDifficultProfileViewModel>()
                .ForMember(nameof(MazeDifficultProfileViewModel.Author), opt => opt.MapFrom(db => db.Creater.Name))
                .ForMember(nameof(MazeDifficultProfileViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));
            provider.CreateMap<MazeDifficultProfileViewModel, MazeDifficultProfile>();

            provider.CreateMap<BugReportViewModel, BugReport>();
            provider.CreateMap<BugReport, BugReportViewModel>()
                .ForMember(nameof(BugReportViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));

            provider.CreateMap<Game, GameViewModel>()
                .ForMember(nameof(GameViewModel.Username), opt => opt.MapFrom(dbGame => dbGame.Creater.Name))
                .ForMember(nameof(GameViewModel.Age), opt => opt.MapFrom(dbGame => dbGame.Creater.Age))
                .ForMember(nameof(GameViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));
            provider.CreateMap<GameViewModel, Game>();

            provider.CreateMap<MinerField, MinerFieldViewModel>();
            provider.CreateMap<MinerCell, MinerCellViewModel>();

            provider.CreateMap<MazeLevelModel, MazeViewModel>();
            provider.CreateMap<MazeViewModel, MazeLevelModel>();

            provider.CreateMap<CellModel, CellViewModel>();
            provider.CreateMap<CellViewModel, CellModel>();

            provider.CreateMap<MazeLevelModel, MazeLevel>()
                .ConstructUsing(x => inMazeLevel(x))
                .ForMember(maze => maze.Cells, db => db.MapFrom(model => model.Cells))
                .AfterMap((a, b) => 
                { 
                    foreach(var cell in b.Cells)
                    {
                        cell.Maze = b;
                        
                    }
                    TeleportIn TeleportIn = (TeleportIn)b.Cells.SingleOrDefault(c => c is TeleportIn);
                    var TeleportOut = b.Cells.SingleOrDefault(c => c is TeleportOut);
                    if(TeleportIn != null && TeleportOut != null)
                    {
                        TeleportIn.TeleportExit = (ITeleportOut)TeleportOut;
                    }
                });

            provider.CreateMap<MazeLevel, MazeLevelModel>()
                 .ConstructUsing(x => inMazeModel(x))
                 .ForMember(maze => maze.Cells, db => db.MapFrom(model => model.Cells))
                 .AfterMap((a, b) =>
                 {
                     foreach (var cell in b.Cells)
                     {
                         cell.MazeLevel = b;
                     }
                 });

            provider.CreateMap<CellModel, BaseCell>()
                .ConstructUsing(x => inBaseCell(x));

            provider.CreateMap<BaseCell, CellModel>()
                .ConstructUsing(x => inCellModel(x));



            var mapperConfiguration = new MapperConfiguration(provider);

            var mapper = new Mapper(mapperConfiguration);


            services.AddScoped<IMapper>(x => mapper);

        }
        private MazeLevelModel inMazeModel(MazeLevel maze)
        {
            var model = new MazeLevelModel()
            {
                Height = maze.Height,
                Width = maze.Width,
                HeroMaxFatigure = maze.Hero.MaxFatigue,
                HeroMaxHp = maze.Hero.Max_hp,
                HeroNowFatigure = maze.Hero.CurrentFatigue,
                HeroNowHp = maze.Hero.Hp,
                HeroX = maze.Hero.X,
                HeroY = maze.Hero.Y,

              
            };
            return model;
        }
        private MazeLevel inMazeLevel(MazeLevelModel model)
        {
            var maze = new MazeLevel()
            {
                Height = model.Height,
                Width = model.Width,


            };
            maze.Hero = new Hero(model.HeroX, model.HeroY, maze, model.HeroNowHp, model.HeroNowHp);
            return maze;
        }
        private CellModel inCellModel(BaseCell cell)
        {
            var dict = new Dictionary<Type, CellInfo>()
            {
                { typeof(Wall), CellInfo.Wall},
                { typeof(WeakWall), CellInfo.WeakWall},
                { typeof(Ground), CellInfo.Grow},
                { typeof(GoldMine), CellInfo.Goldmine},
                { typeof(Coin), CellInfo.Coin},
                { typeof(Bed),CellInfo.Bed},
                { typeof(Puddle), CellInfo.Puddle},
                { typeof(VitalityPotion), CellInfo.VitalityPotion},
                { typeof(Bless), CellInfo.Bless},
                { typeof(TeleportIn), CellInfo.TeleportIn},
                { typeof(TeleportOut), CellInfo.TeleportOut},
                { typeof(Fountain), CellInfo.Fountain},
                { typeof(Trap), CellInfo.Trap},
                { typeof(HealPotion), CellInfo.HealPotion},
                { typeof(WolfPit), CellInfo.WolfPit},
                { typeof(Tavern), CellInfo.Tavern},
                { typeof(Healer), CellInfo.Healer},

            };
            var model = new CellModel();
            model.X = cell.X;
            model.Y = cell.Y;


            if (cell is VitalityPotion)
            {
                VitalityPotion vit = (VitalityPotion)cell;
                model.Obj1 = vit.AddMaxFatigue;
                model.TypeCell = CellInfo.VitalityPotion;

            }
            else if (cell is Coin)
            {
                Coin vit = (Coin)cell;
                model.Obj1 = vit.CoinCount;
                model.TypeCell = CellInfo.Coin;
            }
            else if (cell is WeakWall)
            {
                WeakWall vit = (WeakWall)cell;
                model.Obj1 = vit._vitalityOfWeakWall;
                model.TypeCell = CellInfo.WeakWall;
            }
            else if (cell is GoldMine)
            {
                GoldMine vit = (GoldMine)cell;
                model.Obj1 = vit.currentGoldMineMp;
                model.TypeCell = CellInfo.Goldmine;
            }
            else
            {
                model.TypeCell = dict[cell.GetType()];
            }

            return model;
        }
        private BaseCell inBaseCell(CellModel model)
        {
            switch (model.TypeCell)
            {
                case CellInfo.Grow:
                    return new Ground(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Wall:
                    return new Wall(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Trap:
                    return new Trap(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Tavern:
                    return new Tavern(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.WeakWall:
                    return new WeakWall(model.X, model.Y, null) { Id = model.Id, _vitalityOfWeakWall = model.Obj1, };
                case CellInfo.Healer:
                    return new Healer(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.VitalityPotion:
                    return new VitalityPotion(model.X, model.Y, null, model.Obj1) { Id = model.Id };
                case CellInfo.Puddle:
                    return new Puddle(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.TeleportIn:
                    return new TeleportIn(model.X, model.Y, null, null) { Id = model.Id };
                case CellInfo.TeleportOut:
                    return new TeleportOut(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Goldmine:
                    return new GoldMine(model.X, model.Y, null) { Id = model.Id, currentGoldMineMp = model.Obj1 };
                case CellInfo.Fountain:
                    return new Fountain(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Coin:
                    return new Coin(model.X, model.Y, null, model.Obj1) { Id = model.Id };
                case CellInfo.HealPotion:
                    return new HealPotion(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Bed:
                    return new Bed(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.Bless:
                    return new Bless(model.X, model.Y, null) { Id = model.Id };
                case CellInfo.WolfPit:
                    return new WolfPit(model.X, model.Y, null) { Id = model.Id };
                default:
                    return new Ground(model.X, model.Y, null) { Id = model.Id };
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
