using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddScoped<MazeDifficultRepository>(x => new MazeDifficultRepository(x.GetService<WebContext>()));

            services.AddScoped<ImageRepository>();
                //{
                //    var webContext = diContainer.GetService<WebContext>();
                //    var repository = new NewsRepository(webContext);
                //    return repository;
                //});

            services.AddScoped<MovieRepository>(diContainer =>
                {
                    var webContext = diContainer.GetService<WebContext>();
                    var repository = new MovieRepository(webContext);
                    return repository;
                });

            RegisterMapper(services);

            services.AddControllersWithViews();
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

            var mapperConfiguration = new MapperConfiguration(provider);

            var mapper = new Mapper(mapperConfiguration);


            services.AddScoped<IMapper>(x => mapper);

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
