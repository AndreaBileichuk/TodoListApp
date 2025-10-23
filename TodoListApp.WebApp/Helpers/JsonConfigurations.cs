using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoListApp.WebApp.Helpers;

public static class JsonConfigurations
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() }
        };
    }
}
