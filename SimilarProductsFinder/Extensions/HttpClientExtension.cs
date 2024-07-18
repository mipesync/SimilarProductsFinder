using System.Text;
using System.Text.Json;

namespace SimilarProductsFinder.Extensions;

/// <summary>
/// Расширения для HttpClient
/// </summary>
public static class HttpClientExtension
{
    /// <summary>
    /// Послать POST запрос с json контентом
    /// </summary>
    /// <param name="httpClient">Текущий HttpClient</param>
    /// <param name="url">Ссылка, на которую будет совершён запрос</param>
    /// <param name="body">Тело запроса</param>
    /// <typeparam name="T">Тип, который вернётся после завершения запроса</typeparam>
    public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string url, StreamContent streamContent)
    {
        var response = await httpClient.PostAsync(url, streamContent);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<T>(responseString);
        return result ?? throw new JsonException($"Не удалось десериализовать ответ в тип {typeof(T)} или пришёл пустой ответ");
    }
}