namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SeachMovies;
internal class SeachMoviesInteractor : ISeachMoviesInputPort
{
    readonly ISearchMovieService<SearchMovieInEnglish> EnglishService;
    readonly ISearchMovieService<SearchMovieInSpanish> SpanishService;
    public SeachMoviesInteractor(
        ISearchMovieService<SearchMovieInEnglish> englishService,
        ISearchMovieService<SearchMovieInSpanish> spanishService)
    {
        EnglishService = englishService;
        SpanishService = spanishService;
    }

    public async Task<IEnumerable<SearchMovieResult>> SearchMovies(string text, string lang)
    {
        IEnumerable<SearchMovieResult> data = new List<SearchMovieResult>();
        if (string.IsNullOrEmpty(text) == false && string.IsNullOrEmpty(lang) == false)
        {
            switch (lang.ToLower())
            {
                case "en":
                    data = await EnglishService.SearchMovies(text);
                    break;
                case "es":
                    data = await SpanishService.SearchMovies(text);
                    break;
            };
        }
        return data;
    }
}
