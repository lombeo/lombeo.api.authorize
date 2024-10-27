using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Lombeo.Api.Authorize.Infra.Constants;
using System.Text;
using System.Text.RegularExpressions;

namespace Api_Project_Prn.Services.GoogleDriveService
{
    public interface IGoogleDriveService
    {
        Task<string> UploadFile(Stream fileStream, string fileName, string mimeType);
        Task<List<string>> UploadFiles(List<(Stream fileStream, string fileName, string mimeType)> files);
        Task DeleteFile(string fileId);
        Task DeleteFilesByLinks(List<string> fileLinks);
    }

    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private readonly string ApplicationName = "Your App Name";
        private readonly DriveService _service;

        public GoogleDriveService()
        {
            GoogleCredential credential;
            var jsonCredentials = Environment.GetEnvironmentVariable("GOOGLE_DRIVE_CREDENTIALS_JSON");
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonCredentials)))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.ScopeConstants.Drive);
            }


            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "eBay FPTU",
            });
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName, string mimeType)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName
            };

            FilesResource.CreateMediaUpload request;
            request = _service.Files.Create(fileMetadata, fileStream, mimeType);
            request.Fields = "id";
            request.Upload();

            var file = request.ResponseBody;

            SetPublicPermission(file.Id);

            // Tạo link để thêm vào database
            var link = $"https://drive.usercontent.google.com/download?id={file.Id}&export=view";
            return link;
        }

        public async Task<List<string>> UploadFiles(List<(Stream fileStream, string fileName, string mimeType)> files)
        {
            var links = new List<string>();

            foreach (var file in files)
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = file.fileName
                };

                FilesResource.CreateMediaUpload request;
                request = _service.Files.Create(fileMetadata, file.fileStream, file.mimeType);
                request.Fields = "id";
                request.Upload();

                var uploadedFile = request.ResponseBody;

                // Đặt quyền chia sẻ công khai cho mỗi file
                SetPublicPermission(uploadedFile.Id);

                // Tạo link download
                var link = $"https://drive.usercontent.google.com/download?id={uploadedFile.Id}&export=view";
                links.Add(link);
            }

            return links;
        }

        private void SetPublicPermission(string fileId)
        {
            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader",
                Type = "anyone"
            };

            _service.Permissions.Create(permission, fileId).Execute();
        }

        public async Task DeleteFile(string fileId)
        {
                _service.Files.Delete(fileId).Execute();
        }

        // Phương thức xóa nhiều file qua link
        public async Task DeleteFilesByLinks(List<string> fileLinks)
        {
            foreach (var link in fileLinks)
            {
                var fileId = ExtractFileIdFromLink(link);
                if (!string.IsNullOrEmpty(fileId))
                {
                    DeleteFile(fileId);
                }
            }
        }

        // Phương thức trích xuất fileId từ link Google Drive
        private string ExtractFileIdFromLink(string link)
        {
            // Sử dụng Regular Expression để tìm fileId từ link
            var regex = new Regex(@"(?:/d/|id=)([a-zA-Z0-9-_]+)");
            var match = regex.Match(link);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}
