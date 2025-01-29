using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Bogus;

namespace Task5.Controllers;

public class BooksController : Controller {
    private static readonly HttpClient HttpClient = new();
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] RequestBooks requestBooks) {
        var books = await GetBooksFromOpenLibrary(requestBooks.Seed, requestBooks.Page);
        return Json(books);
    }

    private async Task<List<Book>> GetBooksFromOpenLibrary(string seed, int page) {
        seed = string.IsNullOrWhiteSpace(seed) ? "default" : seed;
        int offset = GenerateSeedOffset(seed, page);
        string query = "default";
        var faker = new Faker();

        string apiUrl = $"https://openlibrary.org/search.json?q={query}&limit=20&offset={offset}";

        try {
            await Task.Delay(500);

            HttpResponseMessage responseMessage = await HttpClient.GetAsync(apiUrl);

            if (!responseMessage.IsSuccessStatusCode) {
                Console.WriteLine($"Error fetching books: {responseMessage.StatusCode}");
                return new List<Book>();
            }

            string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            //Console.WriteLine($"Open Library API Response: {jsonResponse}");

            var openLibraryResponse = JsonSerializer.Deserialize<OpenLibraryResponse>(jsonResponse, jsonOptions);

            var books = new List<Book>();
            if (openLibraryResponse?.Docs != null) {
                int index = (page - 1) * 20 + 1; ;

                foreach (var doc in openLibraryResponse.Docs) {
                    books.Add(new Book {
                        Index = index++,
                        ISBN = faker.Commerce.Ean13(),
                        Title = doc.Title ?? "Unknown Title",
                        Author = doc.AuthorName?.Length > 0 ? string.Join(", ", doc.AuthorName) : doc.AuthorName?[0],
                        Publisher = faker.Company.CompanyName(),
                        CoverUrl = doc.CoverI != null ? $"https://covers.openlibrary.org/b/id/{doc.CoverI}-M.jpg" : "https://placehold.co/100x150",
                        Reviews = new Random().Next(0, 5),
                        Likes = new Random().Next(0, 10),
                        ReviewTexts = new List<string> { "Amazing book!", "Loved it!", "Could be better." }
                    });
                }
            }

            return books;
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching books: {ex.Message}");
            return new List<Book>();

        }
    }

    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private int GenerateSeedOffset(string seed, int page) {
        var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));
        int hashInt = BitConverter.ToInt32(hashBytes, 0);
        return Math.Abs(hashInt % 1000) + ((page - 1) * 20);
    }
}