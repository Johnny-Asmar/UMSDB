using MediatR;

namespace PCP.Application.Entities.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Domain.Models.User>
{
    public int Id { get; set; }
}