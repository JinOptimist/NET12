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
using Net12.Maze.Cells.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.Maze;
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

            RegisterRepositoriesAuto(services);

            RegisterMapper(services);

            services.AddScoped<UserService>(x =>
                new UserService(x.GetService<UserRepository>(), x.GetService<IHttpContextAccessor>())
            );

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
        }

        private void RegisterRepositoriesAuto(IServiceCollection services)
        {
            var baseRepoType = typeof(BaseRepository<>);

            Assembly
                .GetAssembly(baseRepoType)
                .GetTypes()
                .Where(type =>
                    type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == baseRepoType)
                .ToList()
                .ForEach(x => SmartAddScope(services, x));
        }

        private void SmartAddScope<T>(IServiceCollection services) where T : class
        {
            Type ourType = typeof(T);
            SmartAddScope(services, ourType);
        }

        private void SmartAddScope(IServiceCollection services, Type ourType)
        {
            services.AddScoped(ourType, serviceProvider =>
            {
                var constructor = ourType
                    .GetConstructors()
                    .OrderByDescending(x => x.GetParameters().Length)
                    .First();

                var parameters = constructor
                    .GetParameters()
                    .Select(x =>
                        serviceProvider.GetService(x.ParameterType)
                    )
                    .ToArray();

                return constructor.Invoke(parameters);
            });
        }

        private void RegisterMapper(IServiceCollection services)
        {
            var provider = new MapperConfigurationExpression();

            provider.CreateMap<News, NewsViewModel>()
                .ForMember(nameof(NewsViewModel.NameOfAuthor), opt => opt.MapFrom(dbNews => dbNews.Author.Name))
                .ForMember(nameof(NewsViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Author.GlobalUserRating));
            provider.CreateMap<NewsViewModel, News>();

            provider.CreateMap<NewsComment, NewsCommentViewModel>()
               .ForMember(nameof(NewsCommentViewModel.NameOfAuthor), opt => opt.MapFrom(dbNewsComment => dbNewsComment.Author.Name));
            provider.CreateMap<NewsCommentViewModel, NewsComment>();

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

            provider.CreateMap<Image, ImageViewModel>();

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

            provider.CreateMap<GameDevicesViewModel, GameDevices>();
            provider.CreateMap<GameDevices, GameDevicesViewModel>();

            provider.CreateMap<Game, GameViewModel>()
                .ForMember(nameof(GameViewModel.Username), opt => opt.MapFrom(dbGame => dbGame.Creater.Name))
                .ForMember(nameof(GameViewModel.Age), opt => opt.MapFrom(dbGame => dbGame.Creater.Age))
                .ForMember(nameof(GameViewModel.GlobalUserRating), opt => opt.MapFrom(db => db.Creater.GlobalUserRating));
            provider.CreateMap<GameViewModel, Game>();

            provider.CreateMap<MinerField, MinerFieldViewModel>();
            provider.CreateMap<MinerCell, MinerCellViewModel>();

            provider.CreateMap<MazeLevelWeb, MazeViewModel>();
            provider.CreateMap<MazeViewModel, MazeLevelWeb>();

            provider.CreateMap<MazeCellWeb, CellViewModel>();
            provider.CreateMap<CellViewModel, MazeCellWeb>();

            provider.CreateMap<Perrmission, PermissionViewModel>();
            provider.CreateMap<PermissionViewModel, Perrmission>();
            provider.CreateMap<MazeLevelWeb, MazeLevel>()
                .ConstructUsing(x => inMazeLevel(x))
                .ForMember(maze => maze.Cells, db => db.MapFrom(model => model.Cells))
                .AfterMap((a, b) =>
                {
                    foreach (var cell in b.Cells)
                    {
                        cell.Maze = b;
                    }
                    foreach (var enemy in b.Enemies)
                    {
                        enemy.Maze = b;
                    }

                    var TeleportIn = (TeleportIn)b.Cells.SingleOrDefault(c => c is TeleportIn);
                    var TeleportOut = b.Cells.SingleOrDefault(c => c is TeleportOut);
                    if (TeleportIn != null && TeleportOut != null)
                    {
                        TeleportIn.TeleportExit = (ITeleportOut)TeleportOut;
                    }
                });

            provider.CreateMap<MazeLevel, MazeLevelWeb>()
                 .ConstructUsing(x => inMazeModel(x))
                 .ForMember(maze => maze.Cells, db => db.MapFrom(model => model.Cells))
                 .ForMember(maze => maze.Enemies, db => db.MapFrom(model => model.Enemies))
                 .AfterMap((a, b) =>
                 {
                     foreach (var cell in b.Cells)
                     {
                         cell.MazeLevel = b;
                     }
                     foreach (var enemy in b.Enemies)
                     {
                         enemy.MazeLevel = b;
                     }
                 });

            provider.CreateMap<MazeCellWeb, BaseCell>()
                .ConstructUsing(x => inBaseCell(x));

            provider.CreateMap<BaseCell, MazeCellWeb>()
                .ConstructUsing(x => inCellModel(x));

            provider.CreateMap<MazeEnemyWeb, BaseEnemy>()
                .ConstructUsing(x => inBaseEnemy(x));

            provider.CreateMap<BaseEnemy, MazeEnemyWeb>()
                .ConstructUsing(x => inEnemyWeb(x));


            var mapperConfiguration = new MapperConfiguration(provider);

            var mapper = new Mapper(mapperConfiguration);


            services.AddScoped<IMapper>(x => mapper);

        }
        private MazeLevelWeb inMazeModel(MazeLevel maze)
        {
            var model = new MazeLevelWeb()
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
        private MazeLevel inMazeLevel(MazeLevelWeb model)
        {
            var maze = new MazeLevel()
            {
                Height = model.Height,
                Width = model.Width,
                

            };
            maze.Hero = new Hero(model.HeroX, model.HeroY, maze, model.HeroNowHp, model.HeroMaxHp) { Money = model.Creator.Coins, CurrentFatigue = model.HeroNowFatigure};
            return maze;
        }
        private MazeCellWeb inCellModel(BaseCell cell)
        {
            var dict = new Dictionary<Type, MazeCellInfo>()
            {
                { typeof(Wall), MazeCellInfo.Wall},
                { typeof(WeakWall), MazeCellInfo.WeakWall},
                { typeof(Ground), MazeCellInfo.Grow},
                { typeof(GoldMine), MazeCellInfo.Goldmine},
                { typeof(Coin), MazeCellInfo.Coin},
                { typeof(Bed),MazeCellInfo.Bed},
                { typeof(Puddle), MazeCellInfo.Puddle},
                { typeof(VitalityPotion), MazeCellInfo.VitalityPotion},
                { typeof(Bless), MazeCellInfo.Bless},
                { typeof(TeleportIn), MazeCellInfo.TeleportIn},
                { typeof(TeleportOut), MazeCellInfo.TeleportOut},
                { typeof(Fountain), MazeCellInfo.Fountain},
                { typeof(Trap), MazeCellInfo.Trap},
                { typeof(HealPotion), MazeCellInfo.HealPotion},
                { typeof(WolfPit), MazeCellInfo.WolfPit},
                { typeof(Tavern), MazeCellInfo.Tavern},
                { typeof(Healer), MazeCellInfo.Healer},

            };
            var model = new MazeCellWeb();
            model.X = cell.X;
            model.Y = cell.Y;


            if (cell is VitalityPotion)
            {
                model.Obj1 = ((VitalityPotion)cell).AddMaxFatigue;
                model.TypeCell = MazeCellInfo.VitalityPotion;
            }
            else if (cell is Coin)
            {
                model.Obj1 = ((Coin)cell).CoinCount;
                model.TypeCell = MazeCellInfo.Coin;
            }
            else if (cell is WeakWall)
            {
                model.Obj1 = ((WeakWall)cell)._vitalityOfWeakWall;
                model.TypeCell = MazeCellInfo.WeakWall;
            }
            else if (cell is GoldMine)
            {
                model.Obj1 = ((GoldMine)cell).currentGoldMineMp;
                model.TypeCell = MazeCellInfo.Goldmine;
            }
            else
            {
                model.TypeCell = dict[cell.GetType()];
            }

            return model;
        }
        private BaseCell inBaseCell(MazeCellWeb model)
        {
            switch (model.TypeCell)
            {
                case MazeCellInfo.Grow:
                    return new Ground(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Wall:
                    return new Wall(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Trap:
                    return new Trap(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Tavern:
                    return new Tavern(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.WeakWall:
                    return new WeakWall(model.X, model.Y, null) { Id = model.Id, _vitalityOfWeakWall = model.Obj1, };
                case MazeCellInfo.Healer:
                    return new Healer(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.VitalityPotion:
                    return new VitalityPotion(model.X, model.Y, null, model.Obj1) { Id = model.Id };
                case MazeCellInfo.Puddle:
                    return new Puddle(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.TeleportIn:
                    return new TeleportIn(model.X, model.Y, null, null) { Id = model.Id };
                case MazeCellInfo.TeleportOut:
                    return new TeleportOut(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Goldmine:
                    return new GoldMine(model.X, model.Y, null) { Id = model.Id, currentGoldMineMp = model.Obj1 };
                case MazeCellInfo.Fountain:
                    return new Fountain(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Coin:
                    return new Coin(model.X, model.Y, null, model.Obj1) { Id = model.Id };
                case MazeCellInfo.HealPotion:
                    return new HealPotion(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Bed:
                    return new Bed(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.Bless:
                    return new Bless(model.X, model.Y, null) { Id = model.Id };
                case MazeCellInfo.WolfPit:
                    return new WolfPit(model.X, model.Y, null) { Id = model.Id };
                default:
                    return new Ground(model.X, model.Y, null) { Id = model.Id };
            }
        }
        private BaseEnemy inBaseEnemy(MazeEnemyWeb enemyWeb)
        {
            switch (enemyWeb.TypeEnemy)
            {
                case MazeEnemyInfo.Goblin:
                    return new Goblin(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id };
                case MazeEnemyInfo.BullEnemy:
                    return new BullEnemy(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id, _heroDirection = (Direction)enemyWeb.Obj1 };
                case MazeEnemyInfo.Geyser:
                    return new Geyser(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id };
                case MazeEnemyInfo.Slime:
                    return new Slime(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id, Hp = enemyWeb.Obj1 };
                case MazeEnemyInfo.Walker:
                    return new Walker(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id, _rotation = (Direction)enemyWeb.Obj1 };
                case MazeEnemyInfo.Wallworm:
                    return new Wallworm(enemyWeb.X, enemyWeb.Y, null) { Id = enemyWeb.Id, CounterStep = enemyWeb.Obj1, StepsBeforeEating = enemyWeb.Obj2, };
            }
            return null;
        }

        private MazeEnemyWeb inEnemyWeb(BaseEnemy enemy)
        {
            var dict = new Dictionary<Type, MazeEnemyInfo>()
            {
                  {typeof(BullEnemy), MazeEnemyInfo.BullEnemy},
                  {typeof(Geyser),    MazeEnemyInfo.Geyser},
                  {typeof(Goblin),    MazeEnemyInfo.Goblin},
                  {typeof(Slime),     MazeEnemyInfo.Slime},
                  {typeof(Walker),    MazeEnemyInfo.Walker},
                  {typeof(Wallworm),  MazeEnemyInfo.Wallworm},
            };
            var enemyWeb = new MazeEnemyWeb();
            enemyWeb.X = enemy.X;
            enemyWeb.Y = enemy.Y;
            if (enemy is Wallworm)
            {
                enemyWeb.Obj1 = ((Wallworm)enemy).CounterStep;
                enemyWeb.Obj2 = ((Wallworm)enemy).StepsBeforeEating;
            }
            else if (enemy is Slime)
            {
                enemyWeb.Obj1 = ((Slime)enemy).Hp;
            }
            else if (enemy is BullEnemy)
            {
                enemyWeb.Obj1 = (int)((BullEnemy)enemy)._heroDirection;
            }
            else if (enemy is Walker)
            {
                enemyWeb.Obj1 = (int)((Walker)enemy)._rotation;
            }

            enemyWeb.TypeEnemy = dict[enemy.GetType()];
            return enemyWeb;
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
