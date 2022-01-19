namespace Spirebyte.Services.Projects.Application;

public interface IAppContext
{
    string RequestId { get; }
    IIdentityContext Identity { get; }
}