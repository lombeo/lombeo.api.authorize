using Lombeo.Api.Authorize.Infra.Constants;
using StackExchange.Redis;

namespace Lombeo.Api.Authorize.Services.CacheService
{
    public class RedisConnManager
    {
        public RedisConnManager()
        {
            lock (locker)
            {
                if (lazyConnection == null)
                {
                    lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                    {
                        return ConnectionMultiplexer.Connect(StaticVariable.RedisConfig.PubSubConnection);
                    });
                }
            }
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static readonly object locker = new object();
        public ConnectionMultiplexer Connection { get { return lazyConnection.Value; } }
    }
}
