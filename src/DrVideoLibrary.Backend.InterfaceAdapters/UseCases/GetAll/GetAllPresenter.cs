namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetAll;
internal class GetAllPresenter : BaseListPresenter<ListCard>, IGetAllOutputPort
{
    public GetAllPresenter(IFileContent fileContent) : base(fileContent) { }

}
