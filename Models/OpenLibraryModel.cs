using System.Text.Json.Serialization;

namespace Task5.Models;

public class OpenLibraryDoc {
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("author_name")]
    public string[] AuthorName { get; set; }

    [JsonPropertyName("publisher")]
    public string[] Publisher { get; set; }

    [JsonPropertyName("isbn")]
    public string[] Isbn { get; set; }

    [JsonPropertyName("cover_i")]
    public int? CoverI { get; set; }
}

public class OpenLibraryResponse {
    public List<OpenLibraryDoc> Docs { get; set; }
}