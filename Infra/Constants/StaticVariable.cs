﻿using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.Configuration;
using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.Infra.Constants
{
    public static class StaticVariable
    {
        public static int TimeToday = 0;
        public static RedisConfigDTO RedisConfig = AppSettings.Get<RedisConfigDTO>("RedisConfiguration");
        public static JwtValidationDTO JwtValidation = AppSettings.Get<JwtValidationDTO>("JwtValidation");

        public static ScheduleDefaultDTO ScheduleDefault = AppSettings.Get<ScheduleDefaultDTO>("ScheduleDefault");

        public static bool IsInitializedUser = false;
        public static IEnumerable<User> UserMemory = new List<User>();

        public static string GoogleDriveCredentials = Environment.GetEnvironmentVariable("GOOGLE_DRIVE_CREDENTIALS_JSON");
        public static string GoogleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        public static string GoogleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
    }
}
