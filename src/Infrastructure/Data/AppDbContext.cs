using Core.Entities;
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
    }
}
