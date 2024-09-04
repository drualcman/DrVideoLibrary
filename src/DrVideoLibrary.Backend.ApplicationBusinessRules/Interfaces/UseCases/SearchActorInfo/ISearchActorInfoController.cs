namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SearchActorInfo;
public interface ISearchActorInfoController
{
    Task<SearchPersonResult> SearchActor(string name);
}
