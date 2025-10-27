var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");

builder.AddProject<Projects.RealEstateAgency_Api_Host>("realestateagency-api-host")
       .WithReference(mongo);

builder.Build().Run();