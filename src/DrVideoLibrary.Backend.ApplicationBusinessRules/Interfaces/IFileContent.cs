namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IFileContent
{
    Task<string> UploadFile(byte[] content, string filename, string id);
    Task<Uri> GetUri(string filename);
    Task DeleteFile(string filename);
    Task<byte[]> GetUrlBytesAsync(string url);
}
