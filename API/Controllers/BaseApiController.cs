using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] 
[EnableCors("any")]
public class BaseApiController : ControllerBase
{
    
}
