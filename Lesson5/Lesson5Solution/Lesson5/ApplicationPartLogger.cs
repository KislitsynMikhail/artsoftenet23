using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting;

namespace Lesson5;

public class ApplicationPartLogger : BackgroundService
{
    private readonly ApplicationPartManager _partManager;

    /// <inheritdoc />
    public ApplicationPartLogger(ApplicationPartManager partManager)
    {
        _partManager = partManager;
    }

    /// <inheritdoc />
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Get the names of all the application parts. This is the short assembly name for AssemblyParts
        var applicationParts = _partManager.ApplicationParts.Select(x => x.Name);

        // Create a controller feature, and populate it from the application parts
        

        // Log the application parts and controllers

        return Task.CompletedTask;
    }
}