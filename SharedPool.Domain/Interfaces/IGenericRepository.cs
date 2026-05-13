using SharedPool.Domain.Entities;
using System.Linq.Expressions;

namespace SharedPool.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate); // LINQ sorguları için
        Task<T> AddAsync(T entity);
        void Update(T entity); // EF Core'da Update asenkron değildir, sadece state değiştirir.
        void Delete(T entity);
    }
}
