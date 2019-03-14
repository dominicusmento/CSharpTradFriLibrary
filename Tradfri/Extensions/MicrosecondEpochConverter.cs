﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    public class MicrosecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return DateTime.MinValue; }
            return _epoch.AddMilliseconds((long)reader.Value*1000);
        }
    }
}
