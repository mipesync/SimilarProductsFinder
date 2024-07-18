using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SimilarProductsFinder.Extensions;
using SimilarProductsFinder.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SimilarProductsFinder;

/// <summary>
/// Менеджер для HTTP запросов
/// </summary>
public class HttpManager
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Инициализация менеджера
    /// </summary>
    public HttpManager()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Получить похожие товары
    /// </summary>
    /// <param name="filePath">Путь до файла изображения, по которому необходимо осуществить поиск</param>
    /// <returns><see cref="SimilarProductsResponse"/></returns>
    public async Task<SimilarProductsResponse> GetSimilarProductsAsync(string filePath)
    {
        const string url = "https://www.aliexpressimagesearch.com/api/imagesearch";
        
        using var content = new MultipartFormDataContent();

        var fileStreamInfo = await GetFileStreamAsync(filePath);
        var fileStreamContent = new StreamContent(fileStreamInfo.Stream);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue($"image/{fileStreamInfo.FileExtension.Trim('.')}");
        content.Add(fileStreamContent, name: "image", fileStreamInfo.FileName);

        var response = await _httpClient.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Статус код ответа: {response.StatusCode}");
        if (response.StatusCode != HttpStatusCode.OK)
            Console.WriteLine($"Текст ошибки: {responseString}");
        
        return JsonSerializer.Deserialize<SimilarProductsResponse>(responseString) 
            ?? throw new JsonException($"Не удалось десериализовать ответ в тип {typeof(SimilarProductsResponse)} или пришёл пустой ответ");
    }

    private async Task<FileStreamInfo> GetFileStreamAsync(string filePath)
    {
        var fileStreamInfo = new FileStreamInfo();
        
        if (filePath.Contains("http"))
        {
            fileStreamInfo.Stream = await _httpClient.GetStreamAsync(filePath);
            fileStreamInfo.FileName = Path.GetFileName(new Uri(filePath).AbsolutePath);
            fileStreamInfo.FileExtension = Path.GetExtension(new Uri(filePath).AbsolutePath);
        }
        else
        {
            fileStreamInfo.Stream = File.OpenRead(filePath);
            fileStreamInfo.FileName = Path.GetFileName(filePath);
            fileStreamInfo.FileExtension = Path.GetExtension(filePath);
        }

        fileStreamInfo.Stream = await ResizeImage(fileStreamInfo.Stream);

        return fileStreamInfo;
    }

    private async Task<Stream> ResizeImage(Stream fileStream)
    {
        using var image = await Image.LoadAsync(fileStream);
        
        if (image.Height > image.Width)
            image.Mutate(r => r.Resize(0, 900));
        if (image.Width > image.Height)
            image.Mutate(r => r.Resize(900, 0));
        if (image.Width == image.Height)
            image.Mutate(r => r.Resize(900, 900));
        
        var outputStream = new MemoryStream();
        
        await image.SaveAsync(outputStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
        
        outputStream.Position = 0;
        
        return outputStream;
    }
}