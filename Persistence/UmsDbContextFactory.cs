using Microsoft.EntityFrameworkCore;


namespace Persistence
{
    public class UmsDbContextFactory 
        : DesignTimeDbContextFactoryBase<UmsContext>
    {
        protected override UmsContext CreateNewInstance(DbContextOptions<UmsContext> options)
        {
            return new UmsContext(options);
        }
    }
}