﻿namespace DrVideoLibrary.Backend.Repositories.Interfaces;
public interface IMoviesContext
{
    Task AddMovie(MovieModel data);
    Task<IEnumerable<MovieModel>> GetMoviesAll();
    Task RegisterWatchingNow(RegisterView data);
    Task<RegisterView> GetWatchingNow();
    Task<IEnumerable<RegisterView>> GetTotalViews(string movieId);
    Task<MovieModel> GetMovieById(string id);
    Task<IEnumerable<WatchedModel>> GetWatchedMoviesAll();
    Task<IEnumerable<WatchedCountModel>> GetWatchedViewsAll();
}
