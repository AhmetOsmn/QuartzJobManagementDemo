using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace QuartzJobManagementDemo.Extensions
{
    public static class SerilogExtensions
    {      
        public static WebApplicationBuilder UseMySerilog(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Host.UseSerilog((context, services, configuration) => configuration
             .Enrich.WithMachineName()
             .Enrich.FromLogContext()
             .WriteTo.Debug()
             .WriteTo.Console()
             .WriteTo.Elasticsearch(CreateElasticsearchSinkOptions(webApplicationBuilder.Configuration))
             .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
             .ReadFrom.Configuration(webApplicationBuilder.Configuration)
         );

            return webApplicationBuilder;
        }

        private static ElasticsearchSinkOptions CreateElasticsearchSinkOptions(IConfiguration configuration)
        {
            return new ElasticsearchSinkOptions(
            [
                new(uriString: configuration.GetSection("Elasticsearch:Uri").Value ?? throw new ArgumentNullException("Elasticsearch:Uri"))
            ])
            {
                IndexFormat = $"{configuration.GetSection("Elasticsearch:DefaultIndex").Value}-{DateTime.UtcNow:yyyy-MM}",
                ModifyConnectionSettings = (c) => c.BasicAuthentication(configuration.GetSection("Elasticsearch:Username").Value, configuration.GetSection("Elasticsearch:Password").Value),
                MinimumLogEventLevel = LogEventLevel.Information,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
        }
    }
}
