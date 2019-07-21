﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.CloudEvents
{
    using Newtonsoft.Json;

    /// <summary>
    /// A cloud event with binary data.
    /// </summary>
    public class BinaryCloudEventV0_1 : CloudEventV0_1
    {
#pragma warning disable CA1819 // Properties should not return arrays
        /// <summary>
        /// Gets or sets the binary payload.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public byte[] Data { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
