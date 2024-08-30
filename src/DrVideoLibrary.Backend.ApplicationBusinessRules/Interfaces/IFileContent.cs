namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IFileContent
{
    Task<string> UploadFile(byte[] content, string filename, string id);
    Task<Uri> GetUri(string filename, int days = 0);
    Task DeleteFile(string filename);
    Task<byte[]> GetUrlBytesAsync(string url);
}
