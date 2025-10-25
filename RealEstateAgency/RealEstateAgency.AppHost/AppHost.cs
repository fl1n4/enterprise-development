var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RealEstateAgency_Api_Host>("realestateagency-api-host");

builder.Build().Run();
