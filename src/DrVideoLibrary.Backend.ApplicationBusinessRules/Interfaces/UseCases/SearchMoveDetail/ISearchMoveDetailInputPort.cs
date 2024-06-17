namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SearchMoveDetail;
public interface ISearchMoveDetailInputPort
{
    Task<Movie> SearchMoveDetail(string id, string lang);
}
