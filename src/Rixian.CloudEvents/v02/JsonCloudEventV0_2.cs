using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rixian.CloudEvents
{
    public class JsonCloudEventV0_2 : CloudEventV0_2
    {
        // Required
        [JsonRequired]
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 7)]
        public JToken Data { get; set; }
    }
}
