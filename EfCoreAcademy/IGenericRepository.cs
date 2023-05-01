using EfCoreAcademy.Model;
using System.Linq.Expressions;

namespace EfCoreAcademy
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<int> InsertAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
