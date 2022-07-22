using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.User.Commands.AddUser;

public class AddUserCommand : IRequest<UserViewModel>
{
    public string Name { get; set; }
    public string KeyClockId { get; set; }
    public long RoleId { get; set; }
    public string Email { get; set; }
    public int SubscribeToEmail { get; set; }

}