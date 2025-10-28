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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var mongoConnectionString = builder.Configuration.GetConnectionString("mongo")
    ?? throw new InvalidOperationException("MongoDB connection string 'mongo' is missing.");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

const string databaseName = "RealEstateAgency";
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IRealEstateObjectRepository, RealEstateObjectRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

builder.Services.AddScoped<IClientCRUDService, ClientService>();
builder.Services.AddScoped<IRealEstateObjectCRUDService, RealEstateObjectService>();
builder.Services.AddScoped<IRequestCRUDService, RequestService>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();