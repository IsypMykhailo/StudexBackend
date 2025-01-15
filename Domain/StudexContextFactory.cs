using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Studex.Domain;

public class StudexContextFactory : IDesignTimeDbContextFactory<StudexContext>
{
    public StudexContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
        
        var connectionString = configuration.GetValue<string>("Database:ConnectionString");
        var poolSize = configuration.GetValue<int>("Database:PoolSize");
        
        var optionsBuilder = new DbContextOptionsBuilder<StudexContext>();
        optionsBuilder
            .UseNpgsql(connectionString)
            .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        
        return new StudexContext(optionsBuilder.Options);
    }
}