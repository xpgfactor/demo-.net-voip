using Identity.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.DI
{
    public static class ApplicationExtension
    {
        public static IMvcBuilder AddApplication(this IMvcBuilder services)
        {
            services.Services.AddAutoMapper(typeof(AppMappingProfile));

            return services;
        }
    }
}
