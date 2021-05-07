using PlannerApp.Models;
using System.Threading.Tasks;

namespace PlannerApp.Server.Interfaces
{
    public interface IUserService
    {

        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);

        Task<UserManagerResponse> LoginUserAsync(LoginRequest model);
    }


}
