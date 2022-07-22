using System.Reflection;
using Domain.Models;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PCP.Application.Entities.User.Commands.AddUser;
using PCP.Application.Entities.User.Commands.DeleteUser;
using PCP.Application.Entities.User.Commands.UpdateUser;
using PCP.Application.Entities.User.Queries.GetAllStudentOfACourse;
using PCP.Application.Entities.User.Queries.GetAllUsers;
using PCP.Application.Entities.User.Queries.GetUserById;
using PCP.Application.ViewModel;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ODataController
{
    
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    
    [EnableQuery]
    [HttpGet("all")]
    public async Task<List<UserViewModel>> GetAll()
    {
       
        return await _mediator.Send(new GetAllUsersQuery());

    }
    [HttpGet("{id:int}")]
    public async Task<UserViewModel> GetUserById([FromRoute]int id)
    {
        return  await _mediator.Send(new GetUserByIdQuery
        {
            Id = id
        });
        

    }
    [HttpPost()]
    public async Task<IActionResult> AddUser([FromBody]AddUserCommand addUserCommand)
    {
        var users = await _mediator.Send(addUserCommand);
        return Ok(users);
    }
    
    [HttpDelete("DeleteStudent/{id:int}")]
    public async Task<List<User>> DeleteStudent([FromRoute]int id)
    {
        return await _mediator.Send(new DeleteUserCommand()
        {
            Id = id
        });


    }
    [HttpGet("UpdateName/{id:int}")]
    public async Task<List<User>> UpdateUser([FromRoute]int id, [FromQuery]string name)
    {
        return await _mediator.Send(new UpdateUserCommand()
        {
            Id = id,
            name = name
            
        });
        
        


    }
    [HttpGet("allStudentsOfACourse/{courseId:int}")]
    public async Task<List<UserViewModel>> GetAllStudentsOfACourse([FromRoute]int courseId)
    {
       
        return await _mediator.Send(new GetAllStudentsOfACourseQuery
        {
            courseId = courseId
        });

    }
}