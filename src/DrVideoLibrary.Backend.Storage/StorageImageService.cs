namespace DrVideoLibrary.Backend.Storage;
public class StorageImageService : IFileContent
{
    readonly BlobServiceClient ImagesService;
    readonly StorageOptions Options;
    public StorageImageService(IOptions<StorageOptions> options, IOptions<ConnectionStringsOptions> connectionStrings)
    {
        Options = options.Value;
        ImagesService = new BlobServiceClient(connectionStrings.Value.Storage);
    }

    private BlobContainerClient GetImagesClient() =>
        ImagesService.GetBlobContainerClient(Options.ImagesContainer);

    private BlobClient GetImageClient(string fileName)
    {
        BlobContainerClient blobContainerClient = GetImagesClient();
        return blobContainerClient.GetBlobClient(fileName);
    }

    public async Task DeleteFile(string filename)
    {
        if (!string.IsNullOrEmpty(filename))
            await GetImageClient(filename).DeleteIfExistsAsync();
    }

    public async Task<string> UploadFile(byte[] content, string fileName)
    {
        string filename = FilesHerper.GenerateRandomeFileName(Path.GetExtension(fileName));
        try
        {
            BlobClient imageClient = GetImageClient(filename);
            BlobContentInfo response = await imageClient.UploadAsync(FilesHerper.ToStream(content), true);
        }
        catch
        {
            filename = string.Empty;
        }
        return filename;
    }

    public Task<Uri> GetUri(string filename)
    {
        Uri uri = null;
        if (!string.IsNullOrEmpty(filename))
        {
            BlobClient imageClient = GetImageClient(filename);
            uri = imageClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.UtcNow.AddDays(-15).AddMonths(1));
        }
        return Task.FromResult(uri);
    }
}
