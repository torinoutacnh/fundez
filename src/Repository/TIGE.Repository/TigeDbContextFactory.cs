using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TIGE.Repository
{
    public class TigeDbContextFactory: IDesignTimeDbContextFactory<TigeDbContext>
    {
        public TigeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TigeDbContext>();

            TigeDbContextFactoryHelper.Build(null, optionsBuilder);
            
            return new TigeDbContext(optionsBuilder.Options);
        }
    }
}