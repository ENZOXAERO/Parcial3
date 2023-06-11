using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace wsIncidents.Middleware {

    internal static class rateLimitingMiddleware {

        internal static IServiceCollection addRateLimiting(this IServiceCollection services,IConfiguration configuration) {

            //Use to rate limit counters
            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(x => configuration.GetSection("IpRateLimitingSettings").Bind(x));
            services.Configure<IpRateLimitPolicies>(x => configuration.GetSection("IpRateLimitPolicies").Bind(x));

            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            return services;
        }

        internal static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app) {
            app.UseIpRateLimiting();
            return app;
        }
    }
}
