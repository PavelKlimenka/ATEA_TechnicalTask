using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
