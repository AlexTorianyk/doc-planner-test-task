using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;

        var code = StatusCodes.Status500InternalServerError; // Default to 500 if unexpected

        if (exception is BaseHttpException baseHttpException) code = (int)baseHttpException.StatusCode;

        var problemDetails = new ProblemDetails
        {
            Status = code,
            Title = "An error occurred while processing your request.",
            Detail = exception?.Message
        };

        return StatusCode(code, problemDetails);
    }
}
