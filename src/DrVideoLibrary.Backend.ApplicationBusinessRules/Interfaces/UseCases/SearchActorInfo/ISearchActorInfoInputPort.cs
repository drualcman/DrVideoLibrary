namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SearchActorInfo;
public interface ISearchActorInfoInputPort
{
    Task<SearchPersonResult> SearchActor(string name);
}
