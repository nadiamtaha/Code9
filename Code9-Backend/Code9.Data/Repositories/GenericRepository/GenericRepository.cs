using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Code9.Data.Repositories.GenericRepository
{
    /// <summary>
    /// Generic Repository class that can be used in CRUD operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly Code9Context Context;

      
        public GenericRepository(Code9Context dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            Context = dbContext;

        }
        
        public TEntity Get(long id)
        {
            // Here we are working with a DbContext, not PalaghatContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);

        }

        public IEnumerable<TEntity> GetAll()
        {
            
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {

            Context.Entry(entity).State = EntityState.Deleted;

        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public int count(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Count(predicate);
        }

        public void Update(TEntity entityToUpdate)
        {
            Context.Set<TEntity>().Attach(entityToUpdate);

            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<IEnumerable<T>> FindPaged<T>(int page, int pageSize) where T : class
        {
            return await this.Context.Set<T>().ToPagedListAsync(page, pageSize);
        }

        /// <summary>
        /// get data table items count based on viewModel EndString property
        /// </summary>
        /// <param name="endString">Page Length</param>
        /// <returns>items count</returns>
        public static int GetLength(string endString)
        {
            var length = 10;
            var d = !string.IsNullOrEmpty(endString) && int.TryParse(endString, out length);
            if (length <= 0)
            {
                length = 10;
            }

            return length;
        }
        /// <summary>
        /// get data table start index based on StartString
        /// </summary>
        /// <param name="startString">Start page of dataTable</param>
        /// <returns>start index</returns>
        public static int GetStart(string startString)
        {
            int start = 0;
            var d = !string.IsNullOrEmpty(startString) && int.TryParse(startString, out start);
            if (start < 0)
            {
                start = 0;
            }

            return start;
        }
    }
}
