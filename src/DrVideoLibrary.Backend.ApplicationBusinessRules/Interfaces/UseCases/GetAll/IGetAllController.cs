namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetAll;
public interface IGetAllController
{
    Task<IEnumerable<ListCard>> GetAll();
}
