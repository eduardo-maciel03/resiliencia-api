using GitHub.Api.Models;

namespace GitHub.Api.Services
{
    public interface IGitHubService
    {
        Task<GitHubUser> GetByUsernameAsync(string username);
    }
}
