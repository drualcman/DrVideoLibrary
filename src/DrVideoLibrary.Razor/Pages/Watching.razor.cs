using DrVideoLibrary.Entities.Models;

namespace DrVideoLibrary.Razor.Pages;
public partial class Watching
{
    // Add your properties and methods here if needed
    public WatchingNow WatchingNow;

    protected override void OnInitialized()
    {
        WatchingNow = new WatchingNow
        {
            Movie = new Movie
            {
                Title = "The Matrix",
                Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg",
                Released = new DateTime(1999, 3, 31),
                Prologo = "In a dystopian future, humanity is unknowingly trapped inside a simulated reality, the Matrix, created by intelligent machines to distract humans while using their bodies as an energy source.",
                Rate = 9,
                Duration = 136,
                Categories = new List<string> { "Action", "Drama", "Sci-Fi" },
                Directors = new List<string> { "Lana Wachowski", "Lilly Wachowski" },
                Actors = new List<string> { "Keanu Reeves", "Laurence Fishburne", "Carrie-Anne Moss" },
                Relatives = new List<RelativeMovie>
                {
                    new RelativeMovie { Id = "1", Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg", Title = "Algo pasa en las Vegas" },
                    new RelativeMovie { Id = "2", Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg", Title = "Algo pasa en las con Sergi" }
                },
            },
            Start = DateTime.Now.AddMinutes(-75)
        };
    }
}