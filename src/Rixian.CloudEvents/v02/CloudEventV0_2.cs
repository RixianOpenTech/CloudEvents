using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rixian.CloudEvents
{
    public class CloudEventV0_2
    {
        //public const string RFC3339RegexPattern = @"^(?<fullyear>\d{4})-(?<month>0[1-9]|1[0-2])-(?<mday>0[1-9]|[12][0-9]|3[01])T(?<hour>[01][0-9]|2[0-3]):(?<minute>[0-5][0-9]):(?<second>[0-5][0-9]|60)(?<secfrac>\.[0-9]+)?(Z|(\+|-)(?<offset_hour>[01][0-9]|2[0-3]):(?<offset_minute>[0-5][0-9]))$";
        public const string RFC3339RegexPattern = @"^([0-9]+)-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])[Tt]([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]|60)(\.[0-9]+)?(([Zz])|([\+|\-]([01][0-9]|2[0-3]):[0-5][0-9]))$";
        private static Regex rfc3339Regex = new Regex(RFC3339RegexPattern);
        
        public const string RFC2046RegexPattern = @"[a-zA-Z0-9!#$%^&\\*_\\-\\+{}\\|'.`~]+/[a-zA-Z0-9!#$%^&\\*_\\-\\+{}\\|'.`~]+";
        private static Regex rfc2046Regex = new Regex(RFC2046RegexPattern);

        // See: https://stackoverflow.com/a/475217
        public const string Base64RegexPattern = @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
        private static Regex base64Regex = new Regex(Base64RegexPattern);

        // Required
        [JsonRequired]
        [JsonProperty("id", Order = int.MinValue)]
        public string Id { get; set; }

        // Required
        [JsonRequired]
        [JsonProperty("type", Order = int.MinValue + 1)]
        public string Type { get; set; }

        // Required
        [JsonRequired]
        [JsonProperty("specversion", Order = int.MinValue + 2)]
        public string SpecVersion => "0.2";

        // Required
        [JsonRequired]
        [JsonProperty("source", Order = int.MinValue + 3)]
        public Uri Source { get; set; }

        // Optional
        // Serialize RFC 3339
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 4)]
        public DateTimeOffset? Time { get; set; }

        // Optional
        [JsonProperty("schemaurl", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 5)]
        public Uri SchemaUrl { get; set; }

        // Optional
        [JsonProperty("contenttype", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = int.MinValue + 6)]
        public string ContentType { get; set; }

        public static bool ValidateJson(string json) => ValidateJsonDetailed(json).Item1;
        public static bool ValidateJson(JObject jobj) => ValidateJsonDetailed(jobj).Item1;

        public static Tuple<bool, IReadOnlyList<string>> ValidateJsonDetailed(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(json,
                   new JsonSerializerSettings
                   {
                       DateParseHandling = DateParseHandling.None
                   });
                return ValidateJsonDetailed(JObject.FromObject(obj));
            }
            catch (Exception ex)
            {
                return Tuple.Create<bool, IReadOnlyList<string>>(false, new[] { "Failed to parse json." });
            }
        }

        public static Tuple<bool, IReadOnlyList<string>> ValidateJsonDetailed(JObject jobj)
        {
            List<string> errors = new List<string>();
            try
            {
                bool result = true;

                var containsId = jobj.ContainsKey("id");
                var containsType = jobj.ContainsKey("type");
                var containsSpecVersion = jobj.ContainsKey("specversion");
                var containsSource = jobj.ContainsKey("source");
                //var containsSubject = jobj.ContainsKey("subject");
                var containsTime = jobj.ContainsKey("time");
                var containsSchemaUrl = jobj.ContainsKey("schemaurl");
                var containsData = jobj.ContainsKey("data");
                var containsContentType = jobj.ContainsKey("contenttype");
                //var containsDataContentEncoding = jobj.ContainsKey("datacontentencoding");

                var id = jobj["id"]?.ToString();
                var type = jobj["type"]?.ToString();
                var specVersion = jobj["specversion"]?.ToString();
                var source = jobj["source"]?.ToString();
                //var subject = jobj["subject"]?.ToString();
                var time = jobj["time"]?.ToString();
                var schemaUrl = jobj["schemaurl"]?.ToString();
                var data = jobj["data"]?.ToString();
                var contentType = jobj["contenttype"]?.ToString();
                //var dataContentEncoding = jobj["datacontentencoding"]?.ToString();

                //
                // [id]
                // Required, non-empty string
                if (!containsId)
                {
                    result = false;
                    errors.Add("Required field 'id' is missing.");
                }
                else if (id == null)
                {
                    result = false;
                    errors.Add("Required field 'id' is null.");
                }
                else if (string.IsNullOrWhiteSpace(id))
                {
                    result = false;
                    errors.Add("Required field 'id' must contain a value.");
                }

                //
                // [type]
                // Required, non-empty string
                if (!containsType)
                {
                    result = false;
                    errors.Add("Required field 'type' is missing.");
                }
                else if (type == null)
                {
                    result = false;
                    errors.Add("Required field 'type' is null.");
                }
                else if (string.IsNullOrWhiteSpace(type))
                {
                    result = false;
                    errors.Add("Required field 'type' must contain a value.");
                }

                //
                // [specversion]
                // Required, non-empty string set to 0.2
                if (!containsSpecVersion)
                {
                    result = false;
                    errors.Add("Required field 'specversion' is missing.");
                }
                else if (specVersion == null)
                {
                    result = false;
                    errors.Add("Required field 'specversion' is null.");
                }
                else if (string.IsNullOrWhiteSpace(specVersion))
                {
                    result = false;
                    errors.Add("Required field 'specversion' must contain a value.");
                }
                else if (string.Equals(specVersion, "0.2", StringComparison.OrdinalIgnoreCase) == false)
                {
                    result = false;
                    errors.Add("Required field 'specversion' must contain the value '0.2'");
                }

                //
                // [source]
                // Required, non-null Uri
                if (!containsSource)
                {
                    result = false;
                    errors.Add("Required field 'source' is missing.");
                }
                else if (source == null)
                {
                    result = false;
                    errors.Add("Required field 'source' is null.");
                }
                else if (string.IsNullOrWhiteSpace(source))
                {
                    result = false;
                    errors.Add("Required field 'source' must contain a value.");
                }
                else if (Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out Uri sourceUri) == false)
                {
                    result = false;
                    errors.Add("Required field 'source' must contain a valid Uri.");
                }

                ////
                //// [subject]
                //// Optional, non-empty string
                //if (containsSubject)
                //{
                //    if (subject == null)
                //    {
                //        result = false;
                //        errors.Add("Optional field 'subject' is null.");
                //    }
                //    else if (string.IsNullOrWhiteSpace(subject))
                //    {
                //        result = false;
                //        errors.Add("Optional field 'subject' must contain a value.");
                //    }
                //}

                //
                // [time]
                // Optional, non-empty string
                if (containsTime)
                {
                    if (time == null)
                    {
                        result = false;
                        errors.Add("Optional field 'time' is null.");
                    }
                    else if (string.IsNullOrWhiteSpace(time))
                    {
                        result = false;
                        errors.Add("Optional field 'time' must contain a value.");
                    }
                    else if (rfc3339Regex.IsMatch(time) == false)
                    {
                        result = false;
                        errors.Add("Optional field 'time' must adhere to the format specified in RFC 3339.");
                    }
                }

                //
                // [schemaurl]
                // Optional, non-null Uri
                if (containsSchemaUrl)
                {
                    if (schemaUrl == null)
                    {
                        result = false;
                        errors.Add("Optional field 'schemaurl' is null.");
                    }
                    else if (string.IsNullOrWhiteSpace(schemaUrl))
                    {
                        result = false;
                        errors.Add("Required field 'schemaurl' must contain a value.");
                    }
                    else if (Uri.TryCreate(schemaUrl, UriKind.RelativeOrAbsolute, out Uri schemaUri) == false)
                    {
                        result = false;
                        errors.Add("Optional field 'schemaurl' must contain a valid Uri.");
                    }
                }

                //
                // [contenttype]
                // Optional, non-empty string
                if (containsContentType)
                {
                    if (contentType == null)
                    {
                        result = false;
                        errors.Add("Optional field 'contenttype' is null.");
                    }
                    else if (string.IsNullOrWhiteSpace(contentType))
                    {
                        result = false;
                        errors.Add("Optional field 'contenttype' must contain a value.");
                    }
                    else if (rfc2046Regex.IsMatch(contentType) == false)
                    {
                        result = false;
                        errors.Add("Optional field 'contenttype' must adhere to the format specified in RFC 2046.");
                    }
                }

                ////
                //// [datacontentencoding]
                //// Optional, non-empty string
                //if (containsDataContentEncoding)
                //{
                //    if (dataContentEncoding == null)
                //    {
                //        result = false;
                //        errors.Add("Optional field 'datacontentencoding' is null.");
                //    }
                //    else if (string.IsNullOrWhiteSpace(dataContentEncoding))
                //    {
                //        result = false;
                //        errors.Add("Optional field 'datacontentencoding' must contain a value.");
                //    }
                //}

                //
                // [data]
                // Optional, non-empty string
                if (containsData)
                {
                    if (data == null)
                    {
                        result = false;
                        errors.Add("Optional field 'data' is null.");
                    }
                    else if (string.IsNullOrWhiteSpace(data))
                    {
                        result = false;
                        errors.Add("Optional field 'data' must contain a value.");
                    }
                }

                return Tuple.Create<bool, IReadOnlyList<string>>(result, errors);
            }
            catch (Exception ex)
            {
                return Tuple.Create<bool, IReadOnlyList<string>>(false, errors);
            }
        }

        public static CloudEventV0_2 Deserialize(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentOutOfRangeException(nameof(json), "Must supply a string with content.");

            var jobj = JObject.Parse(json);
            return Deserialize(jobj);
        }

        public static CloudEventV0_2 Deserialize(JObject jobj)
        {
            if (jobj == null) throw new ArgumentNullException(nameof(json));
            
            if (!jobj.ContainsKey("data"))
                return jobj.ToObject<CloudEventV0_2>();

            var contentType = jobj.Value<string>("contenttype")?.ToLowerInvariant()?.Trim();

            // SPEC: Section 3.1 - Paragraph 3
            // https://github.com/cloudevents/spec/blob/v0.1/json-format.md#31-special-handling-of-the-data-attribute
            if (contentType != null && (string.Equals(contentType, "application/json", StringComparison.OrdinalIgnoreCase) || contentType.EndsWith("+json")))
            {
                return jobj.ToObject<JsonCloudEventV0_2>();
            }
            else if (jobj.ContainsKey("data"))
            {
                var data = jobj["data"]?.ToString();
                if (base64Regex.IsMatch(data))
                {
                    return jobj.ToObject<BinaryCloudEventV0_2>();
                }
                else
                {
                    return jobj.ToObject<StringCloudEventV0_2>();
                }
            }
            else
            {
                return jobj.ToObject<CloudEventV0_2>();
            }
        }

        public static CloudEventV0_2 CreateGenericCloudEvent(string eventType, Uri source)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new CloudEventV0_2
            {
                Id = Guid.NewGuid().ToString(),
                Time = DateTimeOffset.UtcNow,
                Type = eventType,
                Source = source,
            };
        }

        public const string JsonMimeType = "application/json";
        public const string PlainTextMimeType = "text/plain";
        public const string OctetStreamMimeType = "application/octet-stream";

        public static JsonCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, JToken payload) => 
            CreateCloudEvent(eventType, source, payload, JsonMimeType, null, null);
        public static JsonCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, JToken payload, string subject, Uri schemaUrl) => 
            CreateCloudEvent(eventType, source, payload, JsonMimeType, subject, schemaUrl);
        public static JsonCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, JToken payload, string contentType, string subject, Uri schemaUrl)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new JsonCloudEventV0_2
            {
                Id = Guid.NewGuid().ToString(),
                Time = DateTimeOffset.UtcNow,
                Type = eventType,
                Source = source,
                SchemaUrl = schemaUrl,
                ContentType = contentType,
                Data = payload,
            };
        }

        public static StringCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, string payload) => 
            CreateCloudEvent(eventType, source, payload, PlainTextMimeType, null, null);
        public static StringCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, string payload, string subject, Uri schemaUrl) => 
            CreateCloudEvent(eventType, source, payload, PlainTextMimeType, subject, schemaUrl);
        public static StringCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, string payload, string contentType, string subject, Uri schemaUrl)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new StringCloudEventV0_2
            {
                Id = Guid.NewGuid().ToString(),
                Time = DateTimeOffset.UtcNow,
                Type = eventType,
                Source = source,
                SchemaUrl = schemaUrl,
                ContentType = contentType,
                Data = payload
            };
        }

        public static BinaryCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, byte[] payload) =>
            CreateCloudEvent(eventType, source, payload, OctetStreamMimeType, null, null);
        public static BinaryCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, byte[] payload, string subject, Uri schemaUrl) =>
            CreateCloudEvent(eventType, source, payload, OctetStreamMimeType, subject, schemaUrl);
        public static BinaryCloudEventV0_2 CreateCloudEvent(string eventType, Uri source, byte[] payload, string contentType, string subject, Uri schemaUrl)
        {
            // Should there be some reasonable upper bound on the payload size?
            return new BinaryCloudEventV0_2
            {
                Id = Guid.NewGuid().ToString(),
                Time = DateTimeOffset.UtcNow,
                Type = eventType,
                Source = source,
                SchemaUrl = schemaUrl,
                ContentType = contentType,
                Data = payload,
            };
        }
    }
}
