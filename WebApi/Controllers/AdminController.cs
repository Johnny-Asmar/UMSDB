using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using PCP.Application.Entities.AddCourseByAdmin.Commands.AddCourse;
using PCP.Application.ViewModel;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateCourse")]
    public async Task<IActionResult> InsertCourse([FromBody]AddCourseCommand addCourseCommand)
    {

        var courseViewModel = await _mediator.Send(addCourseCommand);
    
        return Ok(courseViewModel);

    }
    
    

}