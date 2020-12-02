namespace XeMart.Web.Infrastructure.SessionHelpers
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Http;

    public static class SessionHelper
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.Get<T>(key);

            return value == null ? default :
                JsonSerializer.Deserialize<T>(value.ToString());
        }
    }
}
