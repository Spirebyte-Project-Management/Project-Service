using System;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;

namespace Spirebyte.Services.Projects.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddSingleton<IPermissionService, PermissionService>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }

    public static string GetMimeTypeFromBase64(string base64Url)
    {
        var pFrom = base64Url.IndexOf("data:", StringComparison.Ordinal) + "data:".Length;
        var pTo = base64Url.LastIndexOf(";", StringComparison.Ordinal);

        return base64Url.Substring(pFrom, pTo - pFrom);
    }

    public static string GetDataFromBase64(string base64Url)
    {
        var pFrom = base64Url.IndexOf("base64,", StringComparison.Ordinal) + "base64,".Length;

        return base64Url.Substring(pFrom);
    }

    public static double ConvertToUnixTimestamp(this DateTime date)
    {
        var diff = date.ToUniversalTime() - DateTime.UnixEpoch;
        return Math.Floor(diff.TotalSeconds);
    }
}