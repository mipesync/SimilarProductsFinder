using SimilarProductsFinder;
using SimilarProductsFinder.Models;

var httpManager = new HttpManager();

while (true)
{
    try
    {
        var filePath = GetFilePath();

        var result = await httpManager.GetSimilarProductsAsync(filePath);
        PrintUrls(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
    }
}

string GetFilePath()
{
    Console.Write("Вставьте путь/ссылку на файл, по которому необходимо осуществить поиск: ");
    var path = string.Empty;
    
    while (true)
    {
        path = Console.ReadLine();
        
        if (string.IsNullOrEmpty(path) || path.Length < 10) 
            Console.Write("Введите валидный путь/ссылку на файл: ");
        else break;
    }

    return path;
}

void PrintUrls(SimilarProductsResponse similarProductsResponse)
{
    foreach (var productsCard in similarProductsResponse.Cards!)
    {
        Console.WriteLine(productsCard.ImageUrl);
    }
}