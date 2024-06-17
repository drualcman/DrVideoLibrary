namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesPresenter : BaseListPresenter<RelativeMovie>, IGetRelativesOutputPort
{
    public GetRelativesPresenter(IFileContent fileContent) : base(fileContent) { }
}
