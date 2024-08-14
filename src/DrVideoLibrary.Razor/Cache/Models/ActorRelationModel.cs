namespace DrVideoLibrary.Razor.Cache.Models;
internal class ActorRelationModel
{
    [Field(IsAutoIncremental = true, IsKeyPath = true, IsUnique = true)]
    public int Id { get; set; }

    public string Name { get; set; }
    public List<MovieBasicModel> Movies { get; set; }
    public int Count { get; set; }
}
