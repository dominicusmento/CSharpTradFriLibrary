using Newtonsoft.Json;
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
            if (reader.Value == null)
                return DateTime.MinValue;
            else if (CheckNumberOfDigits((long)reader.Value) == 16)
                return _epoch.AddMilliseconds((long)reader.Value / 1000);
            return _epoch.AddMilliseconds((long)reader.Value * 1000);
        }

        private static int CheckNumberOfDigits(long n)
        {
            n = Math.Abs(n);
            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;
            if (n < 10000000000) return 10;
            if (n < 100000000000) return 11;
            if (n < 1000000000000) return 12;
            if (n < 10000000000000) return 13;
            if (n < 100000000000000) return 14;
            if (n < 1000000000000000) return 15;
            return 16;
        }
    }
}
