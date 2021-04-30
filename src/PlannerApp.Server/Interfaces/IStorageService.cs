using Microsoft.AspNetCore.Http;
using PlannerApp.Server.Models;
using System.Threading.Tasks;

namespace PlannerApp.Server.Interfaces
{
    public interface IStorageService
    {
        Task RemoveAsync(string containerName, string url);
        Task SaveBlobAsync(string containerName, IFormFile file, BlobType blobType);
    }


}
