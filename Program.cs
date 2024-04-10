using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Services.CacheService;
using System.ComponentModel.Design;

namespace Lombeo.Api.Authorize
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.ConfigureServices();

            var app = builder.Build().ConfigurePipeline();

            var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<IPubSubService>().SubscribeInternal();

            var utcNow = DateTime.UtcNow;
            int timeToday = utcNow.Year + utcNow.Month + utcNow.Day;
            StaticVariable.TimeToday = timeToday;

            //scope.ServiceProvider.GetService<IDiscussionService>()?.InitDiscussionMemory();
            //scope.ServiceProvider.GetService<IBlogService>()?.InitBlogMemory();
            //scope.ServiceProvider.GetService<IHelpService>()?.InitHelpMemory();
            app.Run();
        }
    }
}
