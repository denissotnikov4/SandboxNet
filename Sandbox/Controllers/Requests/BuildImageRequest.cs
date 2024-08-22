using Newtonsoft.Json;
using Sandbox.Controllers.Requests.Attributes;

namespace Sandbox.Controllers.Requests;

public record struct BuildImageRequest
{
    [JsonProperty("dockerFileDirectory")]
    public required string DockerFileDirectory { get; init; }

    [JsonProperty("imageName")]
    [DockerImageName]
    public required string ImageName { get; init; }
}