//Install EF Core tools: tool install --global dotnet-ef
//Creating migration: dotnet ef migrations ad InitialMigration
// dotnet ef context scaffold "Filename=EfCoreAcademy.db" Microsoft.Entity.Framework.Sqlite
//Instead of filename you could specify another db managment system

using EfCoreAcademy;
using EfCoreAcademy.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite().Options;
var dbContext = new ApplicationDbContext(options);

dbContext.Database.Migrate();

//ProcessDelete();
//ProccessInsert();
//ProcessSelect();
//ProcessUpdate();

ProcessRepository();

void ProcessDelete()
{
    var professors = dbContext.Professors.ToList();
    var students = dbContext.Students.ToList();
    var classes = dbContext.Classes.ToList();
    var addresses = dbContext.Addresses.ToList();

    dbContext.Professors.RemoveRange(professors);
    dbContext.Students.RemoveRange(students);
    dbContext.Classes.RemoveRange(classes);
    dbContext.Addresses.RemoveRange(addresses);

    dbContext.SaveChanges();
    dbContext.Dispose();
}

void ProccessInsert()
{
    var dbContext = new ApplicationDbContext(options);

    var address = new Address()
    {
        City = "Hamburg",
        Street = "Demostreet",
        Zip = "24225",
        HouseNubmber = 1
    };
    var professor = new Professor()
    {
        FirstName = "Jonathon",
        LastName = "Schoolman",
         Address = address
    };

    var student1 = new Student()
    {
        FirstName = "John",
        LastName = "Doe",
        Address = address
    };

    var student2 = new Student()
    {
        FirstName = "Maria",
        LastName = "Marker",
        Address = address
    };

    var class1 = new Class()
    {
        Professor = professor,
        Students = new List<Student> { student1, student2 },
        Title= "IT"
    };

    dbContext.Addresses.Add(address);
    dbContext.Students.Add(student1);
    dbContext.Students.Add(student2);
    dbContext.Professors.Add(professor);
    dbContext.Classes.Add(class1);

    dbContext.SaveChanges();
    dbContext.Dispose();
}

void ProcessSelect()
{
    var dbContext = new ApplicationDbContext(options);
    //var professor = dbContext.Professors.Include(x => x.Address).Single(p => p.FirstName == "Jonathon");
    var student1 = dbContext.Students.Include(c => c.Classes).Where(x => x.FirstName == "Maria").ToList();
    dbContext.Dispose();
}

void ProcessUpdate()
{
    var dbContext = new ApplicationDbContext(options);

    var student1 = dbContext.Students.First();
    student1.FirstName = "Tim";
    dbContext.SaveChanges();

    dbContext.Dispose();

    dbContext = new ApplicationDbContext(options);
    student1 = dbContext.Students.First();
    dbContext.Dispose();
    Console.ReadLine();
}

async void ProcessRepository()
{
    dbContext = new ApplicationDbContext(options);
    var repository = new GenericRepository<Student>(dbContext);

    //simple select
    var students = await repository.GetAsync(null, null);
    var student = await repository.GetByIdAsync(students.First().Id);

    //Includes
    student = await repository.GetByIdAsync(student.Id, (student) => student.Address,
        (student) => student.Classes);

    //Filters
    Expression<Func<Student, bool>> filter = (student) => student.FirstName == "Maria";
    students = await repository.GetFilteredAsync(new[] { filter }, null, null);

    Console.ReadLine();
}
