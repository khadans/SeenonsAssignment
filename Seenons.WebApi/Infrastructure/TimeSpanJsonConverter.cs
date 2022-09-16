using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Seenons.WebApi.Infrastructure
{
    public class TimeSpanJsonConverter: JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) =>
            TimeSpan.ParseExact(
                reader.GetString()!,
                @"hh\:mm\:ss",
                CultureInfo.InvariantCulture
            );

        public override void Write(
            Utf8JsonWriter writer,
            TimeSpan timeSpan,
            JsonSerializerOptions options
        ) =>
            writer.WriteStringValue(timeSpan.ToString(@"hh\:mm\:ss"));
    }
}
