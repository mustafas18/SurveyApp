﻿using Core.Interfaces.IRepositories;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
        }
    }
}