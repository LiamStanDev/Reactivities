using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet] // api/activities
    public async Task<ActionResult<List<Activity>>> GetActivities(
    /*         CancellationToken cancellationToken */
    )
    {
        // Mediator: 為BaseApiController的屬性
        return await Mediator.Send(
            new List.Query() /* , cancellationToken */
        );
    }

    [HttpGet("{id}")] // api/activities/fdsdfasd
    public async Task<IActionResult> GetActivities(Guid id)
    {
        var result = await Mediator.Send(new Details.Query() { Id = id });

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity) // use IActionResult because we dot't need to return something.
    {
        await Mediator.Send(new Create.Command() { Activity = activity });
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity)
    {
        activity.Id = id;
        await Mediator.Send(new Edit.Command() { Activity = activity });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        await Mediator.Send(new Delete.Command() { Id = id });
        return Ok();
    }
}
