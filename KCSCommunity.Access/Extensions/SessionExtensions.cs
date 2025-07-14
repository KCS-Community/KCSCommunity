using System.Text.Json;

namespace KCSCommunity.Access.Extensions;

public static class SessionExtensions
{
    private static readonly JsonSerializerOptions _options = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping//处理特殊字符
    };

    public static void Set<T>(this ISession session, string key, T value)
    {
        session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value, _options));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var data = session.Get(key);
        return data == null ? default : JsonSerializer.Deserialize<T>(data, _options);
    }
}