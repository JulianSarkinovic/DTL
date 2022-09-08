using System.Text.Json;

namespace DikkeTennisLijst.Infrastructure.Serialization
{
    public static class ObjectSerializer
    {
        public static async Task SerializeAsync<T>(Stream ms, T obj)
        {
            await JsonSerializer.SerializeAsync(ms, obj);
            ms.Position = 0;
        }

        public static byte[] SerializeToUtf8Bytes<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static async Task<T> DeserializeAsync<T>(Stream stream)
        {
            stream.Position = 0;
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }
    }
}