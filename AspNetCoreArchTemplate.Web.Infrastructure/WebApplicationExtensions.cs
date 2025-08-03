
namespace PowerNutrition.Web.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Data.Seeding.Interfaces;

    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IIdentitySeeder identitySeeder = serviceProvider
                .GetRequiredService<IIdentitySeeder>();
            identitySeeder
                .SeedRolesAndDefaultManager()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
