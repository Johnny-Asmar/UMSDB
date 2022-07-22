using AutoMapper;
using MediatR;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.User.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    public UmsContext _umsContext;
    private IMapper _mapper;
    
    public GetUserByIdHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = (from c in _umsContext.Users
            where c.Id == request.Id
            select c).First();
        UserViewModel userViewModel = _mapper.Map<UserViewModel>(user);
        
        return userViewModel;
    }
}