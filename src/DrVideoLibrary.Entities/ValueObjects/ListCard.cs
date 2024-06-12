namespace DrVideoLibrary.Entities.ValueObjects;
public class ListCard
{
    public string Id { get; }
    public string Title { get; }
    public string Cover { get; }
    public int Year { get; }
    public IEnumerable<string> Categories { get; }

    public ListCard(string id, string title, string cover, int year, IEnumerable<string> categories)
    {
        Id = id;
        Title = title;
        Cover = cover;
        Year = year;
        Categories = categories;
        Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg";

    }
}
