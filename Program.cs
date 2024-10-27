using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Services.AuthenService;
using Lombeo.Api.Authorize.Services.CacheService;
using System.ComponentModel.Design;

namespace Lombeo.Api.Authorize
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            // Load environment variables from .env
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.ConfigureServices();

            var app = builder.Build().ConfigurePipeline();

            using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Initialize services within the scope
                scope.ServiceProvider.GetService<IPubSubService>()?.SubscribeInternal();
                scope.ServiceProvider.GetService<IAuthenService>()?.InitUserMemory();
            }

            // Setting the static variable
            var utcNow = DateTime.UtcNow;
            int timeToday = utcNow.Year + utcNow.Month + utcNow.Day;
            StaticVariable.TimeToday = timeToday;

            app.Run();
        }
    }

}
