using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using TicTacToe.Domain;
using TicTacToe.Infrastructure;

namespace TicTacToe.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(BoardJsonConverter.Shared);
                });
        return services;
    }

    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TicTacToe HTTP API",
                Version = "v1",
            });
        });

        return services;
    }

    public static IServiceCollection AddConfiguredRedisGameRepository(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RedisGameRepositorySettings>(configuration.GetSection(nameof(RedisGameRepositorySettings)));
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<RedisGameRepositorySettings>>();
            var settings = options.Value;
            var configurationOptions = ConfigurationOptions.Parse(settings.ConnectionString);
            return ConnectionMultiplexer.Connect(configurationOptions);
        });
        services.AddTransient<IGameRepository, RedisGameRepository>();
        return services;
    }
}