namespace DrVideoLibrary.Razor.Cache.Models;
internal class MovieCardModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string Cover { get; set; }
    public int Year { get; set; }
    public string Categories { get; set; }

    public static MovieCardModel FromListCard(ListCard card) =>
        new MovieCardModel
        {
            Id = card.Id,
            Title = card.Title,
            OriginalTitle = card.OriginalTitle,
            Cover = card.Cover,
            Year = card.Year,
            Categories = JsonSerializer.Serialize(card.Categories)
        };
    public static ListCard ToListCard(MovieCardModel card) =>
        new ListCard(card.Id, card.Title, card.OriginalTitle, card.Cover, card.Year, JsonSerializer.Deserialize<IEnumerable<string>>(card.Categories));
}
