using System;
using System.Collections.Generic;
using System.Text;

namespace Rixian.CloudEvents.Tests
{
    public class Utilities
    {
        public static void ValidateCloudEvent(CloudEventV0_1 cloudEvent)
        {
            if (string.IsNullOrWhiteSpace(cloudEvent.EventType))
                throw new Exception("The eventType property is required and cannot be null or empty.");

            if (cloudEvent.EventTypeVersion != null && string.IsNullOrWhiteSpace(cloudEvent.EventType))
                throw new Exception("The eventTypeVersion property must have a value if supplied.");

            if (string.IsNullOrWhiteSpace(cloudEvent.CloudEventsVersion))
                throw new Exception("The cloudEventsVersion property is required and cannot be null or empty.");

            if (cloudEvent.Source == null)
                throw new Exception("The source property is required.");

            if (string.IsNullOrWhiteSpace(cloudEvent.EventId))
                throw new Exception("The eventId property is required and cannot be null or empty.");

            if (cloudEvent.ContentType != null && string.IsNullOrWhiteSpace(cloudEvent.ContentType))
                throw new Exception("The contentType property must have a value if supplied.");

            if (cloudEvent.Extensions != null && cloudEvent.Extensions.HasValues == false)
                throw new Exception("The extensions property must have a value if supplied.");
        }
    }
}
