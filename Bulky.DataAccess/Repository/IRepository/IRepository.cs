using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category or any other generic model on which we want to interact with DBContext

        //CRUD operations
        IEnumerable<T> GetAll();

        //Find operation on DB only works for Id FirstOrDefault uses LINQ and can find by any parameter
        T Get(Expression<Func<T, bool>> filter);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
