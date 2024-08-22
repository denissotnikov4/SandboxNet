using Docker.DotNet;
using Docker.DotNet.Models;
using ICSharpCode.SharpZipLib.Tar;
using Sandbox.Services.Interfaces;

namespace Sandbox.Services;

public class DockerService : IDockerService
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration(
        new Uri("npipe://./pipe/docker_engine"))
        .CreateClient();
    
    public async Task BuildImageAsync(string dockerfileDirectory, string imageName)
    {
        var imageBuildParameters = new ImageBuildParameters
        {
            Tags = new List<string> { imageName },
        };
        
        using var tarball = CreateTarForDockerfileDirectory(dockerfileDirectory);
        using var responseStream = await _dockerClient.Images.BuildImageFromDockerfileAsync(tarball, imageBuildParameters);
        
        await WaitForBuildImageCompletionAsync(responseStream);
    }
    
    public async Task RunContainerAsync(string imageName)
    {
        var response = await _dockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = imageName,
                HostConfig = new HostConfig
                {
                    AutoRemove = true
                }
            });
        
        await _dockerClient.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
    }

    private async Task WaitForBuildImageCompletionAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        await reader.ReadToEndAsync();
    }

    private static Stream CreateTarForDockerfileDirectory(string dockerfileDirectory)
    {
        var tarStream = new MemoryStream();
        var files = Directory.GetFiles(dockerfileDirectory, "*.*", SearchOption.AllDirectories);

        using var archive = new TarOutputStream(tarStream)
        {
            IsStreamOwner = false
        };

        foreach (var file in files)
        {
            var tarName = file.Substring(dockerfileDirectory.Length).Replace('\\', '/').TrimStart('/');
            
            var entry = TarEntry.CreateTarEntry(tarName);
            using var fileStream = File.OpenRead(file);
            entry.Size = fileStream.Length;
            archive.PutNextEntry(entry);
            
            var localBuffer = new byte[32 * 1024];
            while (true)
            {   
                var numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);
                if (numRead <= 0)
                {
                    break;
                }

                archive.Write(localBuffer, 0, numRead);
            }

            archive.CloseEntry();
        }
        archive.Close();

        tarStream.Position = 0;
        return tarStream;
    }
}