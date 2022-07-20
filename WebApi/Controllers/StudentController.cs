using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCP.Application.Entities.Student.Commands.AddStudentToCourse;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]

public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost()]
    public async Task<IActionResult> InsertCourse([FromBody]AddToCourseCommand addToCourseCommand)
    {

        var classViewModel = await _mediator.Send(addToCourseCommand);
    
        return Ok(classViewModel);

    }
}