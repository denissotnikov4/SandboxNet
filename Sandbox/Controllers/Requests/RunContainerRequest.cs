using Newtonsoft.Json;
using Sandbox.Controllers.Requests.Attributes;

namespace Sandbox.Controllers.Requests;

public record struct RunContainerRequest
{
    [JsonProperty("imageName")]
    [DockerImageName]
    public required string ImageName { get; init; }
}