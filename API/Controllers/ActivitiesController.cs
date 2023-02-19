using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;



public class ActivitiesController : BaseApiController
{
    private readonly DataContext _context;
    public ActivitiesController(DataContext context)
    {
            _context = context;
    }

    [HttpGet] // api/activities
    public async Task<ActionResult<List<Activity>>> GetActivities() 
    {
        return await _context.Activities.ToListAsync();
    }

    [HttpGet("{id}")] // api/activities/fdsdfasd
    public async Task<ActionResult<Activity>> GetActivities(Guid id)
    {
        return await _context.Activities.FindAsync(id);
    }
}
