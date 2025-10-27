using Mapster;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Infrastructure.Mongo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMapster();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddScoped<IClientRepository>(sp =>
    (IClientRepository)new MongoRepository<Client, int>(
        sp.GetRequiredService<IMongoDatabase>(),
        "Clients"
    ));

builder.Services.AddScoped<IRealEstateObjectRepository>(sp =>
    (IRealEstateObjectRepository)new MongoRepository<RealEstateObject, int>(
        sp.GetRequiredService<IMongoDatabase>(),
        "RealEstateObjects"
    ));

builder.Services.AddScoped<IRequestRepository>(sp =>
    (IRequestRepository)new MongoRepository<Request, int>(
        sp.GetRequiredService<IMongoDatabase>(),
        "Requests"
    ));

builder.Services.AddScoped<IClientCRUDService, ClientService>();
builder.Services.AddScoped<IRealEstateObjectCRUDService, RealEstateObjectService>();
builder.Services.AddScoped<IRequestCRUDService, RequestService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();