using Domain.Models;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PCP.Application.Entities.User.Commands.AddUser;
using PCP.Application.Entities.User.Commands.DeleteUser;
using PCP.Application.Entities.User.Commands.UpdateUser;
using PCP.Application.Entities.User.Queries.GetAllUsers;
using PCP.Application.Entities.User.Queries.GetUserById;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ODataController
{
    
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
        
    }
    
    
    [EnableQuery]
    [HttpGet("all")]
    public async Task<List<User>> GetAll()
    {
        
        return await _mediator.Send(new GetAllUsersQuery());

    }
    [HttpGet("{id:int}")]
    public async Task<User> GetUserById([FromRoute]int id)
    {
        return  await _mediator.Send(new GetUserByIdQuery
        {
            Id = id
        });
        

    }
    [HttpPost()]
    public async Task<List<User>> AddUser([FromBody]User user)
    {
        return await _mediator.Send(new AddUserCommand
        {
            user = user
        });


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
}