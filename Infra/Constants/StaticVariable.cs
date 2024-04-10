using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.Configuration;

namespace Lombeo.Api.Authorize.Infra.Constants
{
    public static class StaticVariable
    {
        public static int TimeToday = 0;
        public static RedisConfigDTO RedisConfig = AppSettings.Get<RedisConfigDTO>("RedisConfiguration");
        public static JwtValidationDTO JwtValidation = AppSettings.Get<JwtValidationDTO>("JwtValidation");
    }
}
