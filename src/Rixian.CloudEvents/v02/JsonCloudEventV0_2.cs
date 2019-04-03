using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rixian.CloudEvents
{
    public class JsonCloudEventV0_2 : CloudEventV0_2
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public JToken Data { get; set; }
    }
}
