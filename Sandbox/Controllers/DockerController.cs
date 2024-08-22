using Microsoft.AspNetCore.Mvc;
using Sandbox.Controllers.Requests;
using Sandbox.Services.Interfaces;

namespace Sandbox.Controllers;

[Route("api/docker")]
[ApiController]
public class DockerController : ControllerBase
{
    [HttpPost("images/build")]
    public async Task<IActionResult> BuildDockerImage(
        [FromBody] BuildImageRequest request, 
        [FromServices] IDockerService dockerService)
    {
        await dockerService.BuildImageAsync(request.DockerFileDirectory, request.ImageName);
        return Ok();
    }

    [HttpPost("containers/run")]
    public async Task<IActionResult> RunDockerContainer(
        [FromBody] RunContainerRequest request, 
        [FromServices] IDockerService dockerService)
    {
        await dockerService.RunContainerAsync(request.ImageName);
        return Ok();
    }
}