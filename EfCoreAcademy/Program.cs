//Install EF Core tools: tool install --global dotnet-ef
//Creating migration: dotnet ef migrations ad InitialMigration

using EfCoreAcademy;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite().Options;
var dbContext = new ApplicationDbContext(options);

dbContext.Database.Migrate();
