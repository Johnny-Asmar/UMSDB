using Persistence;

namespace CommonFunctions;

public class CheckRole
{
    
    
    public UmsContext _umsContext;

    public CheckRole(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }
    
    public long checkRole(long Id)
    {
        long RoleId = (from u in _umsContext.Users
            where u.Id == Id
            select u.RoleId).First();
        return RoleId;
    }
    
}