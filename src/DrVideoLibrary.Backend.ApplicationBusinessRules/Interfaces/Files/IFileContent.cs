namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.Files;
public interface IFileContent
{
    Task<string> UploadFile(byte[] content, string filename);
    Task<Uri> GetUri(string filename);
    Task DeleteFile(string filename);
}
