var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ChangeDataCapture_ApiService>("apiservice");

builder.AddProject<Projects.ChangeDataCapture_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
