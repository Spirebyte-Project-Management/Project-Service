using Spirebyte.Services.Projects.Application;

namespace Spirebyte.Services.Projects.Infrastructure;

public interface IAppContextFactory
{
    IAppContext Create();
}