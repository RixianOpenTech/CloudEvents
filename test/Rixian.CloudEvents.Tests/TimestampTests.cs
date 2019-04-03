using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Rixian.CloudEvents.Tests
{
    public class TimestampTests
    {
        public const string RFC3339RegexPattern = @"^(?<fullyear>\d{4})-(?<month>0[1-9]|1[0-2])-(?<mday>0[1-9]|[12][0-9]|3[01])T(?<hour>[01][0-9]|2[0-3]):(?<minute>[0-5][0-9]):(?<second>[0-5][0-9]|60)(?<secfrac>\.[0-9]+)?(Z|(\+|-)(?<offset_hour>[01][0-9]|2[0-3]):(?<offset_minute>[0-5][0-9]))$";

        private Regex rfc399Regex = new Regex(RFC3339RegexPattern);

        [Fact]
        public void Test1()
        {
            var evnt = CloudEventV0_1.CreateCloudEvent("test", "1.0", new Uri("http://localhost"), "FooBar");
            var json = JToken.FromObject(evnt);
            var eventTime = json["eventTime"].ToString();
            Assert.Matches(rfc399Regex, eventTime);
        }
    }
}
