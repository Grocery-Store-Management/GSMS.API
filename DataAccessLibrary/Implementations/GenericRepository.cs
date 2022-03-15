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
            List<T> cachedDatas = await cache.GetAsync<List<T>>(typeof(T).ToString());
            if (cachedDatas == null)
            {
                ReduceDbSet();
                cachedDatas = await dbSet.ToListAsync();
                await cache.SetAsync(typeof(T).ToString(), cachedDatas);
            }
            //List<T> cachedDatas = await dbSet.ToListAsync();
            return cachedDatas;
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task SaveChangesToRedis()
        {
            ReduceDbSet();
            List<T> list = dbSet.ToList();
            await cache.SetAsync(typeof(T).ToString(), list);
        }

        private void ReduceDbSet()
        {
            switch (dbSet)
            {
                case DbSet<Brand> brands:
                    foreach (Brand brand in brands)
                    {
                        brand.Stores = null;
                    }
                    break;
                case DbSet<Category> categories:
                    foreach (Category category in categories)
                    {
                        category.Products = null;
                    }
                    break;
                case DbSet<ImportOrder> importOrders:
                    foreach (ImportOrder importOrder in importOrders)
                    {
                        importOrder.Store = null;
                        importOrder.ImportOrderDetails = null;
                    }
                    break;
                case DbSet<ImportOrderDetail> importOrderDetails:
                    foreach (ImportOrderDetail importOrderDetail in importOrderDetails)
                    {
                        importOrderDetail.ImportOrder = null;
                        importOrderDetail.Product = null;
                    }
                    break;
                case DbSet<Product> products:
                    foreach (Product product in products)
                    {
                        product.InverseMasterProduct = null;
                        product.MasterProduct = null;
                        product.Category = null;
                        product.ReceiptDetails = null;
                        product.ProductDetails = null;
                        product.ImportOrderDetails = null;
                    }
                    break;
                case DbSet<ProductDetail> productDetails:
                    foreach (ProductDetail productDetail in productDetails)
                    {
                        productDetail.Product = null;
                    }
                    break;
                case DbSet<Receipt> receipts:
                    foreach (Receipt receipt in receipts)
                    {
                        receipt.Store = null;
                        receipt.ReceiptDetails = null;
                    }
                    break;
                case DbSet<ReceiptDetail> receiptDetails:
                    foreach (ReceiptDetail receiptDetail in receiptDetails)
                    {
                        receiptDetail.Receipt = null;
                        receiptDetail.Product = null;
                    }
                    break;
                case DbSet<Store> stores:
                    foreach (Store store in stores)
                    {
                        store.Brand = null;
                        store.ImportOrders = null;
                        store.Receipts = null;
                    }
                    break;
            }
        }
    }
}
