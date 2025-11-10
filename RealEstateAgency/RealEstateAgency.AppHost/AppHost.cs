var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");

//var batchSize = builder.AddParameter("GeneratorBatchSize");
//var payloadLimit = builder.AddParameter("GeneratorPayloadLimit");
//var waitTime = builder.AddParameter("GeneratorWaitTime");
//var rabbiMqQueue = builder.AddParameter("RabbitMQQueue");

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var generatorHost = builder.AddProject<Projects.RealEstateAgency_Generator_RabbitMq_Host>("realestateagency-generator-rabbitmq-host")
    .WithReference(rabbitMq)
    .WithEnvironment("RabbitMq__QueueName", "real-estate-queue")
    .WaitFor(rabbitMq);
//var generatorHost = builder.AddProject<Projects.RealEstateAgency_Generator_RabbitMq_Host>("realestateagency-generator-rabbitmq-host")
//    .WithReference(rabbitMq)
//    .WaitFor(rabbitMq)
//    .WithEnvironment("Generator:BatchSize", batchSize)
//    .WithEnvironment("Generator:PayloadLimit", payloadLimit)
//    .WithEnvironment("Generator:WaitTime", waitTime)
//    .WithEnvironment("RabbitMq:QueueName", rabbiMqQueue);

builder.AddProject<Projects.RealEstateAgency_Api_Host>("realestateagency-api-host")
       .WithReference(mongo)
       .WithReference(rabbitMq)
       .WithEnvironment("RabbitMq__QueueName", "real-estate-queue");

builder.Build().Run();