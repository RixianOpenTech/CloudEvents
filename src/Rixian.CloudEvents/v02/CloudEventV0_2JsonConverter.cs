﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;

namespace Rixian.CloudEvents
{
    public class CloudEventV0_2JsonConverter : CustomCreationConverter<CloudEventV0_2>
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(CloudEventV0_2))
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;


                // Load JObject from stream
                JObject jobj = JObject.Load(reader);
                var cloudEvent = CloudEventV0_2.Deserialize(jobj);

                if (existingValue != null && existingValue is CloudEventV0_2 existingEvent)
                {
                    existingEvent.Id = cloudEvent.Id;
                    existingEvent.Source = cloudEvent.Source;
                    existingEvent.SchemaUrl = cloudEvent.SchemaUrl;
                    existingEvent.ContentType = cloudEvent.ContentType;
                    existingEvent.Time = cloudEvent.Time;
                    existingEvent.Type = cloudEvent.Type;

                    // TODO: Data field
                }

                return cloudEvent;
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override CloudEventV0_2 Create(Type objectType)
        {
            return Activator.CreateInstance(objectType) as CloudEventV0_2;
        }
    }
}
