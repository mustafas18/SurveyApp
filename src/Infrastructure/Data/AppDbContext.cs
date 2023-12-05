using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<SheetPage> SheetPages { get; set; }
        public DbSet<UserSurvey> UserSurveys { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Variable> Variables { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Add-Migration <Migration-Name> -Project Infrastructure

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SurveyPlatform;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Language>()
                .HasData(new Language(1,"English", TextDirectionEnum.ltr),
                   new Language(2, "Persian", TextDirectionEnum.rlt));
            modelBuilder.Entity<UserDegree>()
            .HasData(
                new UserDegree(1,"دانشجوی کارشناسی", "BSc Student"),
                new UserDegree(2,"کارشناسی", "BSc"),
                new UserDegree(3,"دانشجوی کارشناسی ارشد", "MSc Student"),
                new UserDegree(4,"کارشناسی ارشد", "MSc"),
                new UserDegree(5,"دانشجوی دکتری", "Ph.D Student"),
                new UserDegree(6,"دکتری", "Ph.D"));
                        
        }
    }
}
