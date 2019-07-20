using Newtonsoft.Json;

namespace Rixian.CloudEvents
{
    public class BinaryCloudEventV0_1 : CloudEventV0_1
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public byte[] Data { get; set; }
    }
}
