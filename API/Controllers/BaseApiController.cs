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
    // 放在這邊主要是不想要每種Controller都要從IOC容器中重新獲得mediator
    private IMediator _mediator;

    // 這是一個Properties，可以直接用arrow來表示getter method
    // ??= 表示前面為空就assign後面的
    // HttpContext為ControllerBase的properties，為ControllerBase中信息最豐富的
    // 裡面包含Request, Respone, User(裡面包含用戶與驗證信息等內容), Session, Items(當前請求的暫存數據), Services(IServiceProvider實例)
    // HttpContext.RequestServices 返回IServiceProvider
    // GetRequireService() : 若沒有得到Service拋出異常
    // GetService() : 若沒有返回null
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null)
        {
            return NotFound();
        }
        if (result.IsSuccess && result.Value != null)
        {
            return Ok(result.Value);
        }
        if (result.IsSuccess && result.Value == null)
        {
            return NotFound();
        }
        return BadRequest();
    }
}
