namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SearchMoveDetail;
internal class SearchMoveDetailInteractor : ISearchMoveDetailInputPort
{
    readonly ISearchMovieService<SearchMovieInEnglish> EnglishService;
    readonly ISearchMovieService<SearchMovieInSpanish> SpanishService;
    public SearchMoveDetailInteractor(
        ISearchMovieService<SearchMovieInEnglish> englishService,
        ISearchMovieService<SearchMovieInSpanish> spanishService)
    {
        EnglishService = englishService;
        SpanishService = spanishService;
    }

    public async Task<Movie> SearchMoveDetail(string id, string lang)
    {
        Movie movie = null;
        if (string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(lang) == false)
        {
            switch (lang.ToLower())
            {
                case "en":
                    movie = await EnglishService.GetMovieDetails(id);
                    break;
                case "es":
                    movie = await SpanishService.GetMovieDetails(id);
                    break;
            };
        }
        return movie;
    }
}
