namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesPresenter : BaseListPresenter<MovieRelationDto>, IGetRelativesOutputPort
{
    public GetRelativesPresenter(IFileContent fileContent) : base(fileContent) { }
}
