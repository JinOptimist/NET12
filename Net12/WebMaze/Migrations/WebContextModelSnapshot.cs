﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebMaze.EfStuff;

namespace WebMaze.Migrations
{
    [DbContext(typeof(WebContext))]
    partial class WebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PerrmissionUser", b =>
                {
                    b.Property<long>("PerrmissionsId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersWhichHasThePermissionId")
                        .HasColumnType("bigint");

                    b.HasKey("PerrmissionsId", "UsersWhichHasThePermissionId");

                    b.HasIndex("UsersWhichHasThePermissionId");

                    b.ToTable("PerrmissionUser");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Book", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicationDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.BugReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.ToTable("BugReports");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Game", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("YearOfProd")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.ToTable("FavGames");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.GameDevices", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeDevice")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.ToTable("GameDevices");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Assessment")
                        .HasColumnType("int");

                    b.Property<long?>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Gallery");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeCellWeb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long?>("MazeLevelId")
                        .HasColumnType("bigint");

                    b.Property<int>("Obj1")
                        .HasColumnType("int");

                    b.Property<int>("Obj2")
                        .HasColumnType("int");

                    b.Property<int>("TypeCell")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MazeLevelId");

                    b.ToTable("CellsModels");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeDifficultProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoinCount")
                        .HasColumnType("int");

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("HeroMaxFatigue")
                        .HasColumnType("int");

                    b.Property<int>("HeroMaxHp")
                        .HasColumnType("int");

                    b.Property<int>("HeroMoney")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.ToTable("MazeDifficultProfiles");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeEnemyWeb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long?>("MazeLevelId")
                        .HasColumnType("bigint");

                    b.Property<int>("Obj1")
                        .HasColumnType("int");

                    b.Property<int>("Obj2")
                        .HasColumnType("int");

                    b.Property<int>("TypeEnemy")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MazeLevelId");

                    b.ToTable("MazeEnemyWeb");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeLevelWeb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("HeroMaxFatigure")
                        .HasColumnType("int");

                    b.Property<int>("HeroMaxHp")
                        .HasColumnType("int");

                    b.Property<int>("HeroNowFatigure")
                        .HasColumnType("int");

                    b.Property<int>("HeroNowHp")
                        .HasColumnType("int");

                    b.Property<int>("HeroX")
                        .HasColumnType("int");

                    b.Property<int>("HeroY")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("MazeLevelsUser");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MinerCell", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("FieldId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBomb")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<int>("NearBombsCount")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("MinerCell");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MinerField", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("GamerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GamerId");

                    b.ToTable("MinerField");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.NewCellSuggestion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApproverId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FatigueChange")
                        .HasColumnType("int");

                    b.Property<int>("HealtsChange")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MoneyChange")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("CreaterId");

                    b.ToTable("NewCellSuggestions");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.News", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.NewsComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long?>("NewsId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("NewsId");

                    b.ToTable("NewsComments");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Perrmission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Perrmissions");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.StuffForHero", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<long?>("ProposerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProposerId");

                    b.ToTable("StuffsForHero");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.SuggestedEnemys", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApproverId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CreaterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("CreaterId");

                    b.ToTable("SuggestedEnemys");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("Coins")
                        .HasColumnType("int");

                    b.Property<int>("GlobalUserRating")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameCell", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FieldId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("ZumaGameCells");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameColor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FieldId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("ZumaGameColors");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameField", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ColorCount")
                        .HasColumnType("int");

                    b.Property<long?>("GamerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GamerId");

                    b.ToTable("ZumaGameFields");
                });

            modelBuilder.Entity("PerrmissionUser", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.Perrmission", null)
                        .WithMany()
                        .HasForeignKey("PerrmissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebMaze.EfStuff.DbModel.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWhichHasThePermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.BugReport", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MyBugReports")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Game", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MyFavGames")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.GameDevices", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MyGameDevices")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Image", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Author")
                        .WithMany("Images")
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeCellWeb", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.MazeLevelWeb", "MazeLevel")
                        .WithMany("Cells")
                        .HasForeignKey("MazeLevelId");

                    b.Navigation("MazeLevel");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeDifficultProfile", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MazeDifficultProfiles")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeEnemyWeb", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.MazeLevelWeb", "MazeLevel")
                        .WithMany("Enemies")
                        .HasForeignKey("MazeLevelId");

                    b.Navigation("MazeLevel");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeLevelWeb", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creator")
                        .WithMany("ListMazeLevels")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MinerCell", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.MinerField", "Field")
                        .WithMany("Cells")
                        .HasForeignKey("FieldId");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MinerField", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Gamer")
                        .WithMany("MinerFields")
                        .HasForeignKey("GamerId");

                    b.Navigation("Gamer");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.NewCellSuggestion", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Approver")
                        .WithMany("CellSuggestionsWhichIAprove")
                        .HasForeignKey("ApproverId");

                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MyCellSuggestions")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Approver");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.News", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Author")
                        .WithMany("MyNews")
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.NewsComment", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Author")
                        .WithMany("NewsComments")
                        .HasForeignKey("AuthorId");

                    b.HasOne("WebMaze.EfStuff.DbModel.News", "News")
                        .WithMany("NewsComments")
                        .HasForeignKey("NewsId");

                    b.Navigation("Author");

                    b.Navigation("News");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Review", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creator")
                        .WithMany("MyReviews")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.StuffForHero", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Proposer")
                        .WithMany("AddedSStuff")
                        .HasForeignKey("ProposerId");

                    b.Navigation("Proposer");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.SuggestedEnemys", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Approver")
                        .WithMany("EnemySuggestedWhichIAprove")
                        .HasForeignKey("ApproverId");

                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MyEnemySuggested")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Approver");

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameCell", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.ZumaGameField", "Field")
                        .WithMany("Cells")
                        .HasForeignKey("FieldId");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameColor", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.ZumaGameField", "Field")
                        .WithMany("Palette")
                        .HasForeignKey("FieldId");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameField", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Gamer")
                        .WithMany()
                        .HasForeignKey("GamerId");

                    b.Navigation("Gamer");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeLevelWeb", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Enemies");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MinerField", b =>
                {
                    b.Navigation("Cells");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.News", b =>
                {
                    b.Navigation("NewsComments");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.User", b =>
                {
                    b.Navigation("AddedSStuff");

                    b.Navigation("CellSuggestionsWhichIAprove");

                    b.Navigation("EnemySuggestedWhichIAprove");

                    b.Navigation("Images");

                    b.Navigation("ListMazeLevels");

                    b.Navigation("MazeDifficultProfiles");

                    b.Navigation("MinerFields");

                    b.Navigation("MyBugReports");

                    b.Navigation("MyCellSuggestions");

                    b.Navigation("MyEnemySuggested");

                    b.Navigation("MyFavGames");

                    b.Navigation("MyGameDevices");

                    b.Navigation("MyNews");

                    b.Navigation("MyReviews");

                    b.Navigation("NewsComments");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.ZumaGameField", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Palette");
                });
#pragma warning restore 612, 618
        }
    }
}
