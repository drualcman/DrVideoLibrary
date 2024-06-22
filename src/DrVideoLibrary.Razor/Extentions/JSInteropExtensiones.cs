namespace DrVideoLibrary.Razor.Extentions;

public static class JSInteropExtensiones
{
    public static async ValueTask<IJSObjectReference> ImportJavascript(this IJSRuntime jsRuntime, string javascriptFile, Assembly executionAssembly)
    {
        try
        {
            string javascriptToLoad = $"./{ContentHelper.ContentPath(executionAssembly)}/{javascriptFile}";
            IJSObjectReference javascript = await jsRuntime.InvokeAsync<IJSObjectReference>("import", javascriptToLoad);
            return javascript;

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}
