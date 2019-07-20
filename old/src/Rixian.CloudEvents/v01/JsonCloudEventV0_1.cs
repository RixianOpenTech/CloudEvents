using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rixian.CloudEvents
{
    public class JsonCloudEventV0_1 : CloudEventV0_1
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public JToken Data { get; set; }
    }
}
