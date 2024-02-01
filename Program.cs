// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OlympusExample;
using OlympusExample.Workers;
using System;
using System.IO;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IMultithreadingExample, MultithreadingExample>()
    .AddSingleton<INetworkLocations, NetworkLocations>()
    .BuildServiceProvider();

var folderStructures = serviceProvider.GetService<INetworkLocations>();
if (folderStructures != null)
{
    folderStructures.CreateFolderStructure();
}

var service = serviceProvider.GetService<IMultithreadingExample>();
if (service != null)
{
    service.DoWork();
}

