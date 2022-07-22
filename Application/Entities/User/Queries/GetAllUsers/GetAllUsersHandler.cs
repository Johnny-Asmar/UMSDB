using AutoMapper;
using MediatR;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.User.Queries.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserViewModel>>
{
    private UmsContext _umsContext ;
    private IMapper _mapper;
    
    public GetAllUsersHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;
    }

    public async Task<List<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Models.User> users = _umsContext.Users.Select(x => x).ToList();
        List<UserViewModel> usersViewModel = _mapper.Map<List<UserViewModel>>(users);
        return usersViewModel;
    }
    
}