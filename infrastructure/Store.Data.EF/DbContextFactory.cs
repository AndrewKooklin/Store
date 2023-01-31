using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Store.Data.EF
{
    internal class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public StoreDbContext Create(Type repositoryType)
        {
            var services = httpContextAccessor.HttpContext.RequestServices;

            var dbContexts = services.GetService<Dictionary<Type, StoreDbContext>>();

            if (!dbContexts.ContainsKey(repositoryType))
            {
                dbContexts[repositoryType] = services.GetService<StoreDbContext>();
            }

            return dbContexts[repositoryType];
        }
    }
}