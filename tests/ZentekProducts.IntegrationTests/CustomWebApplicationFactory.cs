using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZentekProducts.Api.Data;
using ZentekProducts.Api.Services;

namespace ZentekProducts.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            services.Configure<JwtSettings>(options =>
            {
                options.Secret = "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
                options.Issuer = "ZentekProductsApi";
                options.Audience = "ZentekProductsClient";
                options.ExpirationMinutes = 60;
            });
        });
    }
}