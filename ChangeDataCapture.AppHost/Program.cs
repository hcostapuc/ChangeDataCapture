using Aspire.Hosting;
using Microsoft.Data.SqlClient;
using ChangeDataCapture.AppHost;
var builder = DistributedApplication.CreateBuilder(args);

var serverPassword = builder.AddParameter("movies-db-password", "ImN0tRec0mendThisPasswordIt'sJustPoc");
var moviesDbResource = builder.AddSqlServer("movies-server", serverPassword)
                              .AddDatabase("movies-db")
                              .WithCdcEnabledCommand();

builder.AddProject<Projects.ChangeDataCapture_ApiService>("apiservice")
       .WithReference(moviesDbResource)
       .WaitFor(moviesDbResource);

var kafka = builder.AddContainer("kafka", "confluentinc/cp-kafka:latest")
    .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "zookeeper:2181")
    .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "PLAINTEXT://kafka:9092")
    .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
    .WithEndpoint(targetPort: 9092, name: "kafka");

var zookeeper = builder.AddContainer("zookeeper", "confluentinc/cp-zookeeper:latest")
    .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
    .WithEndpoint(targetPort: 2181, name: "zookeeper");

builder.AddContainer("kafka-ui", "provectuslabs/kafka-ui:latest")
    .WithEnvironment("KAFKA_CLUSTERS_0_NAME", "local")
    .WithEnvironment("KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS", "kafka:9092")
    .WithEnvironment("KAFKA_CLUSTERS_0_ZOOKEEPER", "zookeeper:2181")
    .WaitFor(kafka)
    .WaitFor(zookeeper)
    .WithEndpoint(8080, targetPort: 8080, name: "kafka-ui");

builder.AddContainer("debezium-connector", "debezium/connect:2.7.3.Final")
            .WithEnvironment("BOOTSTRAP_SERVERS", "kafka:9092")
    .WithEnvironment("GROUP_ID", "1")
    .WithEnvironment("CONFIG_STORAGE_TOPIC", "docker-connect-configs")
    .WithEnvironment("OFFSET_STORAGE_TOPIC", "docker-connect-offsets")
    .WithEnvironment("STATUS_STORAGE_TOPIC", "docker-connect-status")
    .WithEnvironment("KEY_CONVERTER_SCHEMAS_ENABLE", "false")
    .WithEnvironment("VALUE_CONVERTER_SCHEMAS_ENABLE", "false")
    .WithEnvironment("CONNECT_KEY_CONVERTER", "org.apache.kafka.connect.json.JsonConverter")
    .WithEnvironment("CONNECT_VALUE_CONVERTER", "org.apache.kafka.connect.json.JsonConverter")
    .WithEnvironment("CONNECT_INTERNAL_KEY_CONVERTER", "org.apache.kafka.connect.json.JsonConverter")
    .WithEnvironment("CONNECT_INTERNAL_VALUE_CONVERTER", "org.apache.kafka.connect.json.JsonConverter")
    .WithEnvironment("CONNECT_REST_ADVERTISED_HOST_NAME", "debezium-connector")
    .WithEnvironment("CONNECT_PLUGIN_PATH", "/kafka/connect")
    .WithEnvironment("CONNECTOR_CLASS", "io.debezium.connector.sqlserver.SqlServerConnector")
    .WithEnvironment("CONNECTOR_NAME", "sqlserver-connector")
    .WithEnvironment("TOPIC_PREFIX", "movies-")
    .WithEnvironment("DATABASE_HOSTNAME", "movies-server")
    .WithEnvironment("DATABASE_PORT", "1433")
    .WithEnvironment("DATABASE_USER", "sa")
    .WithEnvironment("DATABASE_PASSWORD", "ImN0tRec0mendThisPasswordIt'sJustPoc")
    .WithEnvironment("DATABASE_DBNAME", "movies-db")
    .WithEnvironment("DATABASE_SERVER_NAME", "movies-server")
    .WithEnvironment("TABLE_INCLUDE_LIST", "dbo.movies")
    .WithEnvironment("DATABASE_HISTORY_KAFKA_BOOTSTRAP_SERVERS", "kafka:9092")
    .WithEnvironment("DATABASE_HISTORY_KAFKA_TOPIC", "schema-changes.movies")
        .WaitFor(kafka)
        .WaitFor(zookeeper)
        .WithEndpoint(targetPort: 8083, name: "debezium");

builder.Build().Run();

