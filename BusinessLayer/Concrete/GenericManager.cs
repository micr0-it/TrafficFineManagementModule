using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    /// <summary>
    /// Basit CRUD işlemleri. Kural gerektiren manager'lar ilgili metodu override eder.
    /// </summary>
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericDal<T> _genericDal;

        public GenericManager(IGenericDal<T> genericDal)
        {
            _genericDal = genericDal;
        }

        /// <summary>
        /// Arada yapılacak bir iş olmadığı için 
        /// async/await yazmadan DAL’ın döndürdüğü Task’ı doğrudan geri veriyoruz;
        /// çağıran taraf yine await eder. 
        /// Override eden sınıflarda kural kontrolü olacağı için orada async kullanacağız.
        /// </summary>

        public virtual Task AddAsync(T t)
        {
            return _genericDal.AddAsync(t);
        }

        public virtual Task DeleteAsync(T t)
        {
            return _genericDal.DeleteAsync(t);
        }

        public virtual Task<T?> GetByIdAsync(int id)
        {
            return _genericDal.GetByIdAsync(id);
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null)
        {
            return _genericDal.GetListAsync(filter);
        }

        public virtual Task UpdateAsync(T t)
        {
            return _genericDal.UpdateAsync(t);
        }
    }
}
