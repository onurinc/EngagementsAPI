using EngagementsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlogAPI.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> dbContextOptions) : base(dbContextOptions)
    {
        var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

        try
        {


        if (databaseCreator != null)
            {
                if (databaseCreator.CanConnect()) databaseCreator.Create();

                if (databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        }

        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);

        }  
    }
}