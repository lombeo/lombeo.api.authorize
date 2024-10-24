using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.Configuration;
using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.Infra.Constants
{
    public static class StaticVariable
    {
        public static int TimeToday = 0;
        public static RedisConfigDTO RedisConfig = AppSettings.Get<RedisConfigDTO>("RedisConfiguration");
        public static JwtValidationDTO JwtValidation = AppSettings.Get<JwtValidationDTO>("JwtValidation");
        
        public static string UtilitiesApiUrl = AppSettings.Get<string>("ExternalSiteConfigs:Utilities:Url");
        public static string UtilitiesApiToken = AppSettings.Get<string>("ExternalSiteConfigs:Utilities:Token");

        public static ScheduleDefaultDTO ScheduleDefault = AppSettings.Get<ScheduleDefaultDTO>("ScheduleDefault");

        public static bool IsInitializedUser = false;
        public static IEnumerable<User> UserMemory = new List<User>();
    }
}
