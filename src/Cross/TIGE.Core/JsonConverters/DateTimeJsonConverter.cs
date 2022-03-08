using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Utils;

namespace TIGE.Core.JsonConverters
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()?.ToSystemDateTime();

            return value?.DateTime ?? reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime dateTimeValue, JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTimeValue.ToString(Formattings.DateTimeFormat));
        }
    }
}