using System.Reflection;
using Elect.Core.ConfigUtils;
using Elect.Data.EF.Utils.ModelBuilderUtils;
using Elect.DI.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace TIGE.Repository
{
    [ScopedDependency(ServiceType = typeof(Elect.Data.EF.Interfaces.DbContext.IDbContext))]
    public sealed partial class TigeDbContext : Elect.Data.EF.Services.DbContext.DbContext
    {
        public TigeDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // [Important] Keep Under Base For Override And Make End Result

            // Scan and apply Config/Mapping for Tables/Entities (from folder "Maps")
            builder.AddConfigFromAssembly<TigeDbContext>(typeof(TigeDbContext).GetTypeInfo().Assembly);

            // Set Delete Behavior as Restrict in Relationship
            builder.DisableCascadingDelete();

            // Convention for Table name
            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}