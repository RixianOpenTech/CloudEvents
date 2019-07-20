using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;

namespace Rixian.CloudEvents
{
    public class CloudEventV0_1
    {
        // Required
        [JsonRequired]
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        // Optional
        [JsonProperty("eventTypeVersion", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string EventTypeVersion { get; set; }

        // Required
        [JsonRequired]
        [JsonProperty("cloudEventsVersion")]
        public string CloudEventsVersion => "0.1";

        // Required
        [JsonRequired]
        [JsonProperty("source")]
        public Uri Source { get; set; }

        // Required
        [JsonRequired]
        [JsonProperty("eventId")]
        public string EventId { get; set; }

        // Optional
        [JsonProperty("schemaUrl", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Uri SchemaUrl { get; set; }

        // Optional
        // Serialize RFC 3339
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("eventTime", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTimeOffset? EventTime { get; set; }

        // Optional
        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ContentType { get; set; }

        // Optional
        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public JToken Extensions { get; set; }

        public static CloudEventV0_1 Deserialize(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentOutOfRangeException(nameof(json), "Must supply a string with content.");

            var jobj = JObject.Parse(json);
            if (!jobj.ContainsKey("data"))
                return jobj.ToObject<CloudEventV0_1>();

            var contentType = jobj.Value<string>("contentType")?.ToLowerInvariant()?.Trim();

            // SPEC: Section 3.1 - Paragraph 3
            // https://github.com/cloudevents/spec/blob/v0.1/json-format.md#31-special-handling-of-the-data-attribute
            if (contentType == "application/json" || contentType.EndsWith("+json"))
                return jobj.ToObject<JsonCloudEventV0_1>();

            try
            {
                // Is there something better than simply trying to deserialize it?
                // Maybe we can inspect the mime type?
                return jobj.ToObject<BinaryCloudEventV0_1>();
            }
            catch
            {
                return jobj.ToObject<StringCloudEventV0_1>();
            }
        }


        public static CloudEventV0_1 CreateGenericCloudEvent(string eventType, string eventTypeVersion, Uri source) => CreateGenericCloudEvent(eventType, eventTypeVersion, source, null);
        public static CloudEventV0_1 CreateGenericCloudEvent(string eventType, string eventTypeVersion, Uri source, JToken extensions)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new CloudEventV0_1
            {
                EventId = Guid.NewGuid().ToString(),
                EventTime = DateTimeOffset.UtcNow,
                EventType = eventType,
                EventTypeVersion = eventTypeVersion,
                Source = source,
                Extensions = extensions
            };
        }

        public const string JsonMimeType = "application/json";
        public const string PlainTextMimeType = "text/plain";
        public const string OctetStreamMimeType = "application/octet-stream";

        public static JsonCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, JToken payload) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, JsonMimeType, null);
        public static JsonCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, JToken payload, string contentType) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, contentType, null);
        public static JsonCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, JToken payload, JToken extensions) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, JsonMimeType, extensions);
        public static JsonCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, JToken payload, string contentType, JToken extensions)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new JsonCloudEventV0_1
            {
                EventId = Guid.NewGuid().ToString(),
                EventTime = DateTimeOffset.UtcNow,
                EventType = eventType,
                EventTypeVersion = eventTypeVersion,
                Source = source,
                ContentType = contentType,
                Data = payload,
                Extensions = extensions
            };
        }

        public static StringCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, string payload) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, PlainTextMimeType, null);
        public static StringCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, string payload, string contentType) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, contentType, null);
        public static StringCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, string payload, JToken extensions) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, PlainTextMimeType, extensions);
        public static StringCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, string payload, string contentType, JToken extensions)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new StringCloudEventV0_1
            {
                EventId = Guid.NewGuid().ToString(),
                EventTime = DateTimeOffset.UtcNow,
                EventType = eventType,
                EventTypeVersion = eventTypeVersion,
                Source = source,
                ContentType = contentType,
                Data = payload,
                Extensions = extensions
            };
        }

        public static BinaryCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, byte[] payload) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, OctetStreamMimeType, null);
        public static BinaryCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, byte[] payload, string contentType) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, contentType, null);
        public static BinaryCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, byte[] payload, JToken extensions) => CreateCloudEvent(eventType, eventTypeVersion, source, payload, OctetStreamMimeType, extensions);
        public static BinaryCloudEventV0_1 CreateCloudEvent(string eventType, string eventTypeVersion, Uri source, byte[] payload, string contentType = "application/octet-stream", JToken extensions = null)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new BinaryCloudEventV0_1
            {
                EventId = Guid.NewGuid().ToString(),
                EventTime = DateTimeOffset.UtcNow,
                EventType = eventType,
                EventTypeVersion = eventTypeVersion,
                Source = source,
                ContentType = contentType,
                Data = payload,
                Extensions = extensions
            };
        }
    }
}
