namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SearchMoveDetail;
public interface ISearchMoveDetailController
{
    Task<Movie> SearchMoveDetail(string id, string lang);
}
