using Mapster;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Data;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Infrastructure.Mongo;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer<PropertyPurpose>(new EnumSerializer<PropertyPurpose>(BsonType.String));
BsonSerializer.RegisterSerializer<PropertyType>(new EnumSerializer<PropertyType>(BsonType.String));
BsonSerializer.RegisterSerializer<RequestType>(new EnumSerializer<RequestType>(BsonType.String));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMapster();

builder.Services.AddSwaggerGen(c =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("RealEstateAgency"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }
});

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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

    var clients = db.GetCollection<Client>("Clients");
    var properties = db.GetCollection<RealEstateObject>("RealEstateObjects");
    var requests = db.GetCollection<Request>("Requests");

    if (!clients.AsQueryable().Any())
    {
        var seed = new RealEstateSeed();
        clients.InsertMany(seed.Clients);
        properties.InsertMany(seed.Properties);
        requests.InsertMany(seed.Requests);
    }
}

app.Run();