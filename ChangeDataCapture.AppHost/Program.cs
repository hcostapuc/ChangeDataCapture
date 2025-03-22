using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var password = builder.AddParameter("SqlServerSaPassword", secret: true);

var moviesDbResource = builder.AddSqlServer("movies-server")
                              
                              .AddDatabase("movies-db");

builder.AddProject<Projects.ChangeDataCapture_ApiService>("apiservice")
       .WithReference(moviesDbResource)
       .WaitFor(moviesDbResource);

//builder.AddProject<Projects.ChangeDataCapture_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService)
//    .WaitFor(apiService);

builder.Build().Run();