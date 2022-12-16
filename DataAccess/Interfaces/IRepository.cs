using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
