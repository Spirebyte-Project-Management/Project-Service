using Microsoft.AspNetCore.Mvc;

namespace Spirebyte.Services.Projects.API.Controllers.Base;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is not null) return Ok(model);

        return NotFound();
    }
}