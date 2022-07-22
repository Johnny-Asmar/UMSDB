namespace WebApi;
//Middleware to read from request the Tanent Id but not working...
/*public class TenantMiddleware <TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private HttpContext _httpContext;
    private UmsContext _umsContext;
    public DbContextOptionsBuilder _dbContextOptionsBuilder { get; set; }
    private List<Tanent> _tanents;
    private IServiceCollection _services;

    public TenantMiddleware(IHttpContextAccessor contextAccessor, UmsContext umsContext, DbContextOptionsBuilder dbContextOptionsBuilder, List<Tanent> tanents, IServiceCollection services)
    {
        _umsContext = umsContext;
        _httpContext = contextAccessor.HttpContext;
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _tanents = tanents;
        _services = services;
    }


    private string SetDBName(long tenantId, List<Tanent> tanents)
    {
        string DBName = null;
        Tanent currentTanent = tanents.Where(x => x.Id == tenantId).First();
        if (currentTanent == null)
        {
            throw new Exception("Invalid Tenant!");
        }
        else
        {
            DBName = currentTanent.Name;
        }

        return DBName;

    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (_httpContext != null)
        {
            if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantIdString))
            {
                long tanentId = long.Parse(tenantIdString); //to long
                string dbName = SetDBName(tanentId, _tanents);
                _services.AddDbContext<UmsContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString($"Host=localhost;Port=5432;Database={dbName};Username=postgres;Password=123456")));
                var response = await next();

                return response;
            }
            else
            {
                throw new Exception("Invalid Tenant!");
                
            }

        }
        var response1 = await next();
        return response1;
    }


}
*/