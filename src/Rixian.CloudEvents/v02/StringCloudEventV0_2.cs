using Newtonsoft.Json;

namespace Rixian.CloudEvents
{

    public class StringCloudEventV0_2 : CloudEventV0_2
    {
        // Required
        [JsonRequired]
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 8)]
        public string Data { get; set; }
    }
}
