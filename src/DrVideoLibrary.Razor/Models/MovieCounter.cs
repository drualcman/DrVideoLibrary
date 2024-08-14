namespace DrVideoLibrary.Razor.Models;
internal class MovieCounter
{
    public string Name { get; set; }
    public MovieBasic[] Movies { get; set; }
    public int Count => Movies?.Length ?? 0;
}
