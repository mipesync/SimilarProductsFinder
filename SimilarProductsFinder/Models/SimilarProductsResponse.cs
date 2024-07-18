using System.Text.Json.Serialization;

namespace SimilarProductsFinder.Models;

/// <summary>
/// Модель ответа от API
/// </summary>
public class SimilarProductsResponse
{
    /// <summary>
    /// Успешность выполнения запроса
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    /// <summary>
    /// Список полученных карточек
    /// </summary>
    [JsonPropertyName("cards")]
    public List<SimilarProductsCard>? Cards { get; set; }
}

/// <summary>
/// Вспомогательная модель ответа от API для карточки
/// </summary>
public class SimilarProductsCard
{
    /// <summary>
    /// Ссылка на изображение
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }
    /// <summary>
    /// Цена
    /// </summary>
    [JsonPropertyName("price")]
    public string? Price { get; set; }
    /// <summary>
    /// Название товара
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    /// <summary>
    /// Ссылка на товар
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; } 
}