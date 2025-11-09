using BookStore.Application.Contracts.BookAuthors;
using BookStore.Application.Contracts.Protos;
using BookStore.Infrastructure.RabbitMq;

namespace RealEstateAgency.Api.Host;
/// <summary>
/// Класс-расширение для регистрации в di-контейнере подходящего клиента для сервиса генерации
/// </summary>
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Регистрирует клиент для взаимодейсвия с сервисом генерации данных
    /// </summary>
    /// <param name="builder">Веб-билдер приложения</param>
    /// <param name="configuration">Конфигурация</param>
    /// <returns>Веб-билдер приложения с зареганными службами</returns>
    /// <exception cref="ArgumentNullException">Если параметр конфигурации Generator не найден</exception>
    /// <exception cref="FormatException">Если параметр конфигурации Generator неизвестен</exception>
    public static WebApplicationBuilder AddGeneratorService(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (!configuration.GetSection("Generator").Exists()) throw new ArgumentNullException("Generator", "Generator section is missing");

        _ = configuration["Generator"] switch
        {
            "RabbitMq" => AddRabbitMq(builder),
            "Kafka" => AddKafka(builder),
            "Nats" => AddNats(builder),
            "Grpc" => AddGrpc(builder),
            _ => throw new FormatException("Unknown parameter in Generator section")
        };
        return builder;
    }

    /// <summary>
    /// Регистрирует клиент брокера сообщений и необходимые для его работы службы
    /// </summary>
    /// <param name="builder">Веб-билдер приложения</param>
    /// <returns>Веб-билдер приложения с зареганными службами RabbitMq</returns>
    private static WebApplicationBuilder AddRabbitMq(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<BookStoreRabbitMqConsumer>();
        builder.AddRabbitMQClient("bookstore-rabbitmq");
        return builder;
    }