using EngagementsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EngagementsAPI.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : 
        base(options)
    {
        // var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        //
        // try
        // {
        //
        //
        // if (databaseCreator != null)
        //     {
        //         if (databaseCreator.CanConnect()) databaseCreator.Create();
        //
        //         if (databaseCreator.HasTables()) databaseCreator.CreateTables();
        //     }
        // }
        //
        // catch (Exception ex) 
        // {
        //     Console.WriteLine(ex.Message);
        //
        // }  
    }
}