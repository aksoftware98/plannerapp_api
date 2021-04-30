using Microsoft.AspNetCore.Http;
using PlannerApp.Server.Models;
using System.Threading.Tasks;

namespace PlannerApp.Server.Interfaces
{
    public interface IStorageService
    {
        Task RemoveAsync(string url);
        Task<string> SaveBlobAsync(IFormFile file, BlobType blobType);
    }


}
