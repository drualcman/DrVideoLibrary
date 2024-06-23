namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWatchList;
internal class GetWatchListPresenter: BaseListPresenter<WatchedCard>, IGetWatchListOutputPort
{  
    public GetWatchListPresenter(IFileContent fileContent) : base(fileContent) { }
}
