using k8s.KubeConfigModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDataCapture.AppHost
{
    public static class SqlServerCdcExtension
    {
        public static IResourceBuilder<SqlServerDatabaseResource> WithCdcEnabledCommand(
        this IResourceBuilder<SqlServerDatabaseResource> builder)
        {
            builder.WithCommand(
                name: "clear-cache",
                displayName: "Clear Cache",
                executeCommand: context => OnInitialServerCreationAsync(builder, context),
                //updateState: OnUpdateResourceState,
                iconName: "AnimalRabbitOff",
                iconVariant: IconVariant.Filled);

            return builder;
        }
        static async Task<ExecuteCommandResult> OnInitialServerCreationAsync(
                IResourceBuilder<SqlServerDatabaseResource> builder,
                ExecuteCommandContext context)
        {

            var connectionString = $"Server=127.0.0.1,62351;User ID=sa;Password=ImN0tRec0mendThisPasswordIt'sJustPoc;TrustServerCertificate=true;Database=movies-db;";
            //var connectionString = builder.Resource.ConnectionStringExpression ??
            //    throw new InvalidOperationException(
            //        $"Unable to get the '{context.ResourceName}' connection string.");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("EXEC sys.sp_cdc_enable_db", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(@"
            EXEC sys.sp_cdc_enable_table
                @source_schema = N'dbo',
                @source_name = N'movies',
                @role_name = NULL", connection))
                {
                    command.ExecuteNonQuery();
                }
            }


            //await using var connection = ConnectionMultiplexer.Connect(connectionString);

            //var database = connection.GetDatabase();

            //await database.ExecuteAsync("FLUSHALL");

            return CommandResults.Success();
        }
    }
}
