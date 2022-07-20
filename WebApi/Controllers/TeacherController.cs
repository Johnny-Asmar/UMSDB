using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCP.Application.Entities.Course.Commands.RegisterCourse;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]

public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost()]
    public async Task<IActionResult> InsertCourse([FromBody]RegisterCourseCommand registerCourseCommand)
    {

        var classViewModel = await _mediator.Send(registerCourseCommand);
    
        return Ok(classViewModel);

    }
}