using Spirebyte.Services.Projects.Application.Contexts;

namespace Spirebyte.Services.Projects.Infrastructure.Contexts;

public interface IAppContextFactory
{
    IAppContext Create();
}