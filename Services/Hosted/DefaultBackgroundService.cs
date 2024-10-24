﻿using Lombeo.Api.Authorize.Infra.Constants;
//using Lombeo.Api.Authorize.Services.AuthenService;
using System.ComponentModel.Design;

namespace Lombeo.Api.Authorize.Services.Hosted
{
    public class DefaultBackgroundService : CustomBackgroundService<DefaultBackgroundService>
    {
        public DefaultBackgroundService(ILogger<DefaultBackgroundService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {

        }

        protected override TimeSpan TimeSpanInSecond { get; set; } = TimeSpan.FromSeconds(StaticVariable.ScheduleDefault.ScheduleTimeInSeconds);

        protected override void InternalDoJob()
        {
            if (!StaticVariable.ScheduleDefault.Enabled)
            {
                return;
            }

            var utcNow = DateTime.UtcNow;
            int timeToday = utcNow.Year + utcNow.Month + utcNow.Day;

            using (var scope = ServiceProvider.CreateScope())
            {
                if (timeToday > StaticVariable.TimeToday)
                {
                    try
                    {
                        StaticVariable.IsInitializedUser = false;
                        //scope.ServiceProvider.GetService<IAuthenService>()?.InitUserMemory();

                        StaticVariable.TimeToday = timeToday;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "DefaultBackgroundService.DoJob");
                    }
                }
            }
        }
    }
}
