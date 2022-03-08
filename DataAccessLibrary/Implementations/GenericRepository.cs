using BusinessObjectLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using GsmsLibrary;

namespace DataAccessLibrary.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly GsmsContext context;
        protected readonly DbSet<T> dbSet;
        protected readonly IDistributedCache cache;

        public GenericRepository(GsmsContext context
            , IDistributedCache cache
            )
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            this.cache = cache;
            //dbSet = cache.Get<DbSet<T>>(typeof(T).ToString());
            //if (dbSet == null)
            //{
            //    dbSet = context.Set<T>();
            //    cache.Set(typeof(T).ToString(), dbSet);
            //}
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(string id)
        {
            T entity = GetAsync(id).Result;
            if (entity == null)
            {
                throw new Exception("Entity does not exist!!");
            }
            Delete(entity);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync(string sqlQuery)
        {
            return await dbSet.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            //List<T> cachedDatas = await cache.GetAsync<List<T>>(typeof(T).ToString());
            //if (cachedDatas == null)
            //{
            //    cachedDatas = await dbSet.ToListAsync();
            //    await cache.SetAsync(typeof(T).ToString(), cachedDatas);
            //}
            List<T> cachedDatas = await dbSet.ToListAsync();

            return cachedDatas;
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        //public async Task SaveChangesToRedis()
        //{
        //    await cache.SetAsync(typeof(T).ToString(), dbSet.ToListAsync());
        //}
    }
}
