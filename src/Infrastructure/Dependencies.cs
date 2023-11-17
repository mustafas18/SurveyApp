using Core.Interfaces.IRepositories;
using Core.Interfaces;
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

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped(typeof(IEfRepository<>), typeof(IEfRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVariableRepository, VariableRepository>();
            services.AddScoped<ISheetRepository, SheetRepository>();
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            //services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISheetService, SheetService>();
            services.AddScoped<IVariableService,VariableService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IUserService,UserService>();
        }
    }
}
