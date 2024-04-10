using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO
{
    public class PubSubMessage
    {
        public PubSubEnum PubSubEnum { get; set; }
        public object Data { get; set; }
    }
}
