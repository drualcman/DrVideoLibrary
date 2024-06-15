
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesInteractor : IGetRelativesInputPort
{
    readonly IGetRelativesOutputPort Output;

    public GetRelativesInteractor(IGetRelativesOutputPort output)
    {
        Output = output;
    }

    public async Task Handle(string id)
    {
        var relatives = new List<RelativeMovie>
        {
            new RelativeMovie { Id = "1", Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg", Title = "Algo pasa en las Vegas" },
            new RelativeMovie { Id = "2", Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg", Title = "Algo pasa en las con Sergi" }
        };
        await Output.Handle(relatives);
    }
}
