using Google.Cloud.Storage.V1;

namespace SoframiPaylas.WebUI.ExternalService.StorageService;
public interface IFileService
{
    Task UploadProfilePicture(IFormFile file);
}
public class FileService : IFileService
{

    private async Task<string> UploadFileAsync(IFormFile file, string bucketName, string folder)
    {
        var storageClient = StorageClient.Create();

        var fileName = $"{folder}/{Guid.NewGuid()}_{file.FileName}";
        var storageObject = await storageClient.UploadObjectAsync(
            bucketName,
            fileName,
            file.ContentType,
            file.OpenReadStream());

        var url = $"https://storage.googleapis.com/{bucketName}/{storageObject.Name}";
        return url;
    }
    public async Task UploadProfilePicture(IFormFile file)
    {
        var bucketName = "sofrani-paylas.appspot.com";
        var folder = "Profile Images/"; // Dosyaların yükleneceği klasör
        await UploadFileAsync(file, bucketName, folder);
    }
}