var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var generatorHost = builder.AddProject<Projects.RealEstateAgency_Generator_RabbitMq_Host>("realestateagency-generator-rabbitmq-host")
    .WithReference(rabbitMq)
    .WithEnvironment("RabbitMq__QueueName", "real-estate-queue")
    .WaitFor(rabbitMq);

builder.AddProject<Projects.RealEstateAgency_Api_Host>("realestateagency-api-host")
       .WithReference(mongo)
       .WithReference(rabbitMq)
       .WithEnvironment("RabbitMq__QueueName", "real-estate-queue")
       .WaitFor(rabbitMq);

builder.Build().Run();