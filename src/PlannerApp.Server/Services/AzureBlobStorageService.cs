using Microsoft.AspNetCore.Http;
using PlannerApp.Server.Interfaces;
using PlannerApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PlannerApp.Server.Services
{
    public class AzureBlobStorageService : IStorageService
    {

        private readonly BlobServiceClient _blobClient;
        private readonly IConfiguration _config;

        private BlobContainerClient _container;

        public AzureBlobStorageService(IConfiguration config)
        {
            _config = config;
            _blobClient = new BlobServiceClient(config["AzureBlobSettings:ConnectionString"]);
            _container = _blobClient.GetBlobContainerClient(config["AzureBlobSettings:ContainerName"]);
        }

        public async Task RemoveAsync(string url)
        {
            var fileName = Path.GetFileName(url);
            var blobClient = _container.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> SaveBlobAsync(IFormFile file, BlobType blobType)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = file.FileName;
            string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{Guid.NewGuid()}{extension}";
            ValidateExtension(extension, blobType);

            string url = _config["AzureBlobSettings:Url"];

            await _container.CreateIfNotExistsAsync();
            var blob = _container.GetBlobClient(newFileName);
            using (var stream = file.OpenReadStream())
            {
                var result = await blob.UploadAsync(file.OpenReadStream());
                return $"{url}/{_container.Name}/{newFileName}";
            }
        }

        private void ValidateExtension(string extension, BlobType blobType)
        {
            var allowedImages = new[] { ".jpg", ".png", ".svg", ".bmp", ".jfif" };

            switch (blobType)
            {
                case BlobType.Image:
                    if (!allowedImages.Contains(extension.ToLower()))
                        throw new NotSupportedException($"The extension {extension} is not a valid image file");
                    break;
                case BlobType.Document:
                    break;
                case BlobType.Both:
                    break;
                default:
                    break;
            }
        }
    }
}
