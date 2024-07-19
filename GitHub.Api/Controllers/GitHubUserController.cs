using GitHub.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHub.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class GitHubUserController : ControllerBase
    {
        private readonly IGitHubService _service;

        public GitHubUserController(IGitHubService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _service.GetByUsernameAsync(username);
            return Ok(user);
        }
    }
}
