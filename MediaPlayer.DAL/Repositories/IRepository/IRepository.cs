using System.Linq.Expressions;

namespace MediaPlayer.DAL.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int id);
        //      T Get(Expression<Func<T, bool>> filter);
        //      IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        //void RemoveRange(IEnumerable<T> entity);

        //dang fix
    }
}
