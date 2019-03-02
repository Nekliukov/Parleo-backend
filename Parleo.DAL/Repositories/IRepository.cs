using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Repositories
{
    // if we want to do generic repository T should be interface, that emplements all our models
    public interface IRepository<T> where T : class 
    {
        Action<IList<T>> GetAllAsync();

        Action<T> GetAsync(Guid id);

        Action<bool> AddAsync(T entity);

        Action<bool> UpdateAsync(T entity);

        Action<bool> DeleteAsync(Guid id);
        Action<T> FindByUserNameAsync(string userName);
    }
}
