using Newtonsoft.Json;

namespace Rixian.CloudEvents
{

    public class StringCloudEventV0_2 : CloudEventV0_2
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Data { get; set; }
    }
}
