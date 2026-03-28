using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllAsync();
        Task<IdentityResult> AddAsync(string roleName);
    }
}