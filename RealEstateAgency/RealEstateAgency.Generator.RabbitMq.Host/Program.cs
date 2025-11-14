using RealEstateAgency.Generator.RabbitMq.Host;
using RealEstateAgency.Generator.Services;
using RealEstateAgency.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("rabbitmq",
    configureConnectionFactory: factory =>
    {
        factory.AutomaticRecoveryEnabled = true;
        factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
        factory.TopologyRecoveryEnabled = true;
    });

builder.Services.AddScoped<IProducerService, RealEstateAgencyRabbitMqProducer>();

builder.Services.AddHostedService<ClientGeneratorService>();
builder.Services.AddHostedService<RealEstateObjectGeneratorService>();
builder.Services.AddHostedService<RequestGeneratorService>();

var app = builder.Build();
app.MapDefaultEndpoints();

app.Run();