namespace Sandbox.Services.Interfaces;

public interface IDockerService
{
    public Task BuildImageAsync(string dockerfileDirectory, string imageName);
    
    public Task RunContainerAsync(string imageName);
}