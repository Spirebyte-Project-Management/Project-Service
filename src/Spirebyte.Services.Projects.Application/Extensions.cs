using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();

        public static string GetMimeTypeFromBase64(string base64Url)
        {
            int pFrom = base64Url.IndexOf("data:", StringComparison.Ordinal) + "data:".Length;
            int pTo = base64Url.LastIndexOf(";", StringComparison.Ordinal);

            return base64Url.Substring(pFrom, pTo - pFrom);
        }

        public static string GetDataFromBase64(string base64Url)
        {
            int pFrom = base64Url.IndexOf("base64,", StringComparison.Ordinal) + "base64,".Length;

            return base64Url.Substring(pFrom);
        }
        public static double ConvertToUnixTimestamp(this DateTime date)
        {
            TimeSpan diff = date.ToUniversalTime() - DateTime.UnixEpoch;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
