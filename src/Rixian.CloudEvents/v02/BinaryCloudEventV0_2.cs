using Newtonsoft.Json;

namespace Rixian.CloudEvents
{
    public class BinaryCloudEventV0_2 : CloudEventV0_2
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 7)]
        public byte[] Data { get; set; }
    }
}
