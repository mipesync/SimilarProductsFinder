namespace SimilarProductsFinder.Models;

/// <summary>
/// Информация о файле с потоком
/// </summary>
public class FileStreamInfo
{
    public Stream Stream { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string FileExtension { get; set; } = null!;
}