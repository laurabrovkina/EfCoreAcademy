using EfCoreAcademy.Model;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAcademy
{
    internal class StudentRepository : IStudentRepository
    {
        private ApplicationDbContext DbContext { get; }

        public StudentRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<int> CreateStudentAsync(Student student)
        {
            DbContext.Students.Add(student);
            await DbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await DbContext.Students.FindAsync(studentId);

            if (student != null)
            {
                DbContext.Students.Remove(student);
                await DbContext.SaveChangesAsync();
            }

            throw new InvalidOperationException("Id has not found");
        }

        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await DbContext.Students.FindAsync(studentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await DbContext.Students.ToListAsync();
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            DbContext.Students.Attach(student);
            DbContext.Entry(student).State= EntityState.Modified;
            return await DbContext.SaveChangesAsync();
        }
    }
}
