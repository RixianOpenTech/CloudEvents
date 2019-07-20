using Newtonsoft.Json;

namespace Rixian.CloudEvents
{

    public class StringCloudEventV0_1 : CloudEventV0_1
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Data { get; set; }
    }
}
