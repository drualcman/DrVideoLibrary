namespace DrVideoLibrary.Entities.ValueObjects;
public class ListCard : IMovie
{
    public string Id { get; }
    public string Title { get; }
    public string OriginalTitle { get; set; }
    public string Cover { get; set; }
    public int Year { get; }
    public IEnumerable<string> Categories { get; }

    public ListCard(string id, string title, string originalTitle, string cover, int year, IEnumerable<string> categories)
    {
        Id = id;
        Title = title;
        OriginalTitle = originalTitle;
        Cover = cover;
        Year = year;
        Categories = categories;
    }
}
