using System.Globalization;
using Newtonsoft.Json;

namespace WorkspaceService.Domain.Converters;

public class CustomDateOnlyConverter : JsonConverter<DateOnly>
{
    private static readonly string[] Formats = new[]
    {
        "yyyy-MM-dd", "dd-MM-yyyy", "dd.MM.yyyy", "MM-dd-yyyy", "MM.dd.yyyy",
        "MM.yyyy", "yyyy-MM", "yyyy"
    };

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var str = reader.Value as string;
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new JsonSerializationException("Строка с датой пустая");
        }

        foreach (var format in Formats)
        {
            if (DateTime.TryParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                if (format is "MM.yyyy" or "yyyy-MM")
                {
                    return new DateOnly(date.Year, date.Month, 1);
                }

                if (format is "yyyy")
                {
                    return new DateOnly(date.Year, 1, 1);
                }

                return DateOnly.FromDateTime(date);
            }
        }

        throw new JsonSerializationException("Некорректная дата");
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}