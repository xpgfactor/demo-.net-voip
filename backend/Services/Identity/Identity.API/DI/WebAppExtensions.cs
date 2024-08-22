using AutoMapper;
using Identity.Application.IdentityServerConfig;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.DI
{
    public static class WebAppExtensions
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication webApp)
        {
            using var scope = webApp.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await scope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();

            await RolesCreator.CreateRolesAsync(roleManager);

            using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
            {
                await AddEntitiesIfNotExist(context.Clients, IdentityConfiguration.Clients, (e, d) => e.ClientId.Equals(d.ClientId));
                await AddEntitiesIfNotExist(context.IdentityResources, IdentityConfiguration.IdentityResources, (e, d) => e.Name.Equals(d.Name));
                await AddEntitiesIfNotExist(context.ApiScopes, IdentityConfiguration.ApiScopes, (e, d) => e.Name.Equals(d.Name));
                await AddEntitiesIfNotExist(context.ApiResources, IdentityConfiguration.ApiResources, (e, d) => e.Name.Equals(d.Name));

                await context.SaveChangesAsync();
            }

            return webApp;
        }

        private static async Task AddEntitiesIfNotExist<TEntity, TData>(
            DbSet<TEntity> entities,
            IEnumerable<TData> items,
            Func<TEntity, TData, bool> equals)
            where TEntity : class
            where TData : class
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<IdentityResourceMapperProfile>();
                cfg.AddProfile<ClientMapperProfile>();
                cfg.AddProfile<ApiResourceMapperProfile>();
                cfg.AddProfile<ScopeMapperProfile>();
            }).CreateMapper();

            var all = entities.ToList();
            foreach (var item in items)
            {
                var isNotExist = !all.Any(e => equals(e, item));
                if (isNotExist) await entities.AddAsync(mapper.Map<TEntity>(item));
            }
        }
    }
}
