using RealEstateAgency.Infrastructure.RabbitMq;

namespace RealEstateAgency.Api.Host;

/// <summary>
/// Provides extension methods for configuring generator-related services
/// within the <see cref="WebApplicationBuilder"/>.
/// </summary>
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the generator service based on the configuration defined
    /// in the <c>Generator</c> section of <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="builder">The application builder being configured</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The updated <see cref="WebApplicationBuilder"/> instance</returns>
    public static WebApplicationBuilder AddGeneratorService(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (!configuration.GetSection("Generator").Exists()) throw new ArgumentNullException("Generator", "Generator section is missing");

        _ = configuration["Generator"] switch
        {
            "RabbitMq" => AddRabbitMq(builder),
            _ => throw new FormatException("Unknown parameter in Generator section")
        };
        return builder;
    }

    /// <summary>
    /// Registers RabbitMQ generator-related services, including the consumer
    /// background service and RabbitMQ client configuration
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The updated <see cref="WebApplicationBuilder"/> instance</returns>
    private static WebApplicationBuilder AddRabbitMq(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<RealEstateAgencyRabbitMqConsumer>();
        builder.AddRabbitMQClient("realestateagency-rabbitmq");
        return builder;
    }
}