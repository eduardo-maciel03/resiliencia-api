using GitHub.Api.Models;

namespace GitHub.Api.Services;

public class GitHubService : IGitHubService
{
    private readonly IHttpClientFactory _clientFactory;

    public GitHubService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<GitHubUser> GetByUsernameAsync(string username)
    {
        var client = _clientFactory.CreateClient(GitHubSettings.GitHubApi);

        var response = await client.GetAsync($"users/{username}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<GitHubUser>();
            return content;
        }
        else
        {
            return new GitHubUser();
        }
    }
}
