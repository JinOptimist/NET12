﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebMaze.EfStuff;

namespace WebMaze.Migrations
{
    [DbContext(typeof(WebContext))]
    [Migration("20211121152158_Difficult")]
    partial class Difficult
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameOfAuthor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("News");
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

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.MazeDifficultProfile", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creater")
                        .WithMany("MazeDifficultProfiles")
                        .HasForeignKey("CreaterId");

                    b.Navigation("Creater");
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

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.Review", b =>
                {
                    b.HasOne("WebMaze.EfStuff.DbModel.User", "Creator")
                        .WithMany("MyReviews")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("WebMaze.EfStuff.DbModel.User", b =>
                {
                    b.Navigation("CellSuggestionsWhichIAprove");

                    b.Navigation("MazeDifficultProfiles");

                    b.Navigation("MyCellSuggestions");

                    b.Navigation("MyReviews");
                });
#pragma warning restore 612, 618
        }
    }
}