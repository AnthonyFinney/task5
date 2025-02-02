namespace Task5.Models;

public class Book {
    public int? Index { get; set; }
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public string? CoverUrl { get; set; }
    public int? Likes { get; set; }
    public int? Reviews { get; set; }
    public List<string>? ReviewTexts { get; set; }
}

public class RequestBooks {
    public string Seed { get; set; }
    public int Page { get; set; }
    public string Language { get; set; }
}
