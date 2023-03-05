using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("CorsPolicy")]
public class BaseApiController : ControllerBase
{
    // 放在這邊主要是不想要每個Controller都要從IOC容器中重新獲得mediator
    private IMediator _mediator;

    // 這是一個Properties，可以直接用arrow來表示getter method
    // ??= 表示前面為空就assign後面的
    // HttpContext為ControllerBase的properties，為ControllerBase中信息最豐富的
    // 可以取得服務以及可以用來取得關於Http請求得信息
    // HttpContext.RequestServices 返回IServiceProvider
    // GetRequireService() : 若沒有得到Service拋出異常
    // GetService() : 若沒有返回null
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Value != null)
        {
            return Ok(result.Value);
        }
        if (result.IsSuccess && result.Value == null)
        {
            return NotFound();
        }
        return BadRequest(result.Error);
    }
}

