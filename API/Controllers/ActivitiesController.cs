using Application.Activities;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AllowAnonymous]
public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetActivities()
    {
        // Mediator: 為BaseApiController的屬性
        var result = await Mediator.Send(new List.Query());
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("{id}")] // api/activities/fdsdfasd
    public async Task<IActionResult> GetActivities(Guid id)
    {
        var result = await Mediator.Send(new Details.Query() { Id = id });
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity) // use IActionResult because we dot't need to return something.
    {
        var result = await Mediator.Send(new Create.Command() { Activity = activity });
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity)
    {
        activity.Id = id;
        var result = await Mediator.Send(new Edit.Command() { Activity = activity });
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        Result<Unit?> result = await Mediator.Send(new Delete.Command() { Id = id });
        return HandleResult(result);
    }
}
