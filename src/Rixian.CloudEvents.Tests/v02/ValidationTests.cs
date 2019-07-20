using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Rixian.CloudEvents.Tests.v02
{
    public class ValidationTests
    {
        [Fact]
        public void Basic_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }


        #region id Field Tests
        [Fact]
        public void Id_Guid_Success()
        {
            var json = @"
{
  'id': '00717CE8-D29E-4C2E-84D4-A9E026575778',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext'
}
";
            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void Id_Empty_Fail()
        {
            var json = @"
{
  'id': '',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext'
}
";
            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Id_None_Fail()
        {
            var json = @"
{
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext'
}
";
            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion

        #region type Field Tests
        [Fact]
        public void Type_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': '',
  'specversion': '0.2',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Type_None_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'specversion': '0.2',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion

        #region specversion Field Tests
        [Fact]
        public void SpecVersion_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void SpecVersion_None_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void SpecVersion_Incorrect_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.1',
  'source': '/mycontext'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion

        #region source Field Tests
        [Fact]
        public void Source_FullUri_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': 'https://example.com/foo'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void Source_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': ''
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Source_None_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Source_Invalid_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '`~!@#$%^&*()-_=+[{]};:'"",<.>/?'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion



        #region time Field Tests
        [Fact]
        public void Time_Basic_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'time': '2019-04-13T15:07:00.2031033+00:00'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void Time_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'time': ''
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Time_Invalid_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'time': 'ABCDEFG'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void Time_NoTimeZone_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'time': '2019-04-13T15:07:00.2031033'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion

        #region schemaurl Field Tests
        [Fact]
        public void SchemaUrl_FullUri_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'schemaurl': 'https://example.com/foo'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void SchemaUrl_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'schemaurl': ''
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Fact]
        public void SchemaUrl_Invalid_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'schemaurl': '`~!@#$%^&*()-_=+[{]};:'"",<.>/?'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion


        #region contenttype Field Tests
        [Fact]
        public void ContentType_Basic_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'contenttype': 'text/plain'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void ContentType_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'contenttype': ''
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

        [Theory]
        [InlineData("aaaaaa")]
        [InlineData("pdf")]
        [InlineData("application\\pdf")]
        public void ContentType_Invalid_Fail(string contentType)
        {
            var json = $@"
{{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'contenttype': '{contentType}'
}}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }
        #endregion


        #region data Field Tests
        [Fact]
        public void Data_String_Basic_Success()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'data': 'This is some text...'
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeTrue();
        }

        [Fact]
        public void Data_String_Empty_Fail()
        {
            var json = @"
{
  'id': 'C234-1234-1234',
  'type': 'com.example.someevent',
  'specversion': '0.2',
  'source': '/mycontext',
  'data': ''
}";

            var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
            validationResults.Item1.Should().BeFalse();
        }

//        [Fact]
//        public void Data_String_BLNS_Success()
//        {
//            var blns = GetBlns();

//            foreach (var str in blns)
//            {
//                var json = $@"
//{{
//  'id': 'C234-1234-1234',
//  'type': 'com.example.someevent',
//  'specversion': '0.2',
//  'source': '/mycontext',
//  'data': '{str}'
//}}";

//                var validationResults = CloudEventV0_2.ValidateJsonDetailed(json);
//                validationResults.Item1.Should().BeTrue();
//            }
//        }
        #endregion

        private static string[] GetBlns()
        {
            var lines = File.ReadAllLines(@"blns.txt");
            return lines.Where(l => l.StartsWith("#") == false).Where(l => string.IsNullOrEmpty(l) == false).ToArray();
        }
    }
}
