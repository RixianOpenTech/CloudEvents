// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.CloudEvents
{
    using Newtonsoft.Json;

    public class BinaryCloudEventV0_1 : CloudEventV0_1
    {
        // Optional
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public byte[] Data { get; set; }
    }
}
