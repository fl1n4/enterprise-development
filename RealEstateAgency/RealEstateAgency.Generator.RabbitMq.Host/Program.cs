using RealEstateAgency.Generator.RabbitMq.Host;
using RealEstateAgency.Generator.Services;
using RealEstateAgency.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");
builder.Services.AddScoped<IProducerService, RealEstateAgencyRabbitMqProducer>();
builder.Services.AddHostedService<RealEstateObjectGeneratorService>();
builder.Services.AddHostedService<ClientGeneratorService>();
builder.Services.AddHostedService<RequestGeneratorService>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.GetName().Name!.StartsWith("RealEstateAgency"))
    .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();