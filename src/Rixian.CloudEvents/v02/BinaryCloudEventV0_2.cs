using Newtonsoft.Json;

namespace Rixian.CloudEvents
{
    public class BinaryCloudEventV0_2 : CloudEventV0_2
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 9)]
        public byte[] Data { get; set; }

        // Required if data is present
        [JsonProperty("datacontentencoding", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 8)]
        public string DataContentEncoding { get; set; } = "base64";

        public bool ShouldSerializeDataContentEncoding() => Data != null;
    }
}
