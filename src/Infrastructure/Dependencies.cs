using Domain.Interfaces.IRepositories;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ardalis.Specification;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using MediatR;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            //services.AddScoped(typeof(IEfRepository<>), typeof(IEfRepository<>));
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserCategoryRepository, UserCategoryRepository>();
            services.AddScoped<IVariableRepository, VariableRepository>();
            services.AddScoped<ISheetRepository, SheetRepository>();
            services.AddScoped<ISurveyRepository, SurveyRepository>();
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISheetService, SheetService>();
            services.AddScoped<IVariableService,VariableService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IUserService,UserService>();
        }
    }
}
