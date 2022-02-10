using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLibrary.BusinessEntity
{
    public class StoreBusinessEntity
    {
        private IUnitOfWork work; 
        public StoreBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            return await work.Stores.GetAllAsync();
        }

        public async Task<Store> GetStoreAsync(string id)
        {
            return await work.Stores.GetAsync(id);
        }

        public async Task<IEnumerable<Store>> GetStoresAsync(string brandId)
        {
            IEnumerable<Store> stores = await work.Stores.GetAllAsync();
            stores = from store in stores
                     where store.BrandId.Equals(brandId)
                     select store;
            return stores;
        }

        public async Task<Store> AddStoreAsync(Store newStore)
        {
            newStore.Id = GsmsUtils.CreateGuiId();
            newStore.CreatedDate = DateTime.Now;
            newStore.IsDeleted = false;
            await work.Stores.AddAsync(newStore);
            work.Save();
            return newStore;
        }

        public async Task<Store> UpdateStoreAsync(Store updatedStore)
        {
            Store store = await work.Stores.GetAsync(updatedStore.Id);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            store.Name = updatedStore.Name;
            store.IsDeleted = updatedStore.IsDeleted;
            store.BrandId = updatedStore.BrandId;
            work.Stores.Update(store);
            work.Save();
            return store;
        }

        public async Task DeleteStoreAsync(string id)
        {
            Store store = await work.Stores.GetAsync(id);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            work.Stores.Delete(store);
            work.Save();
        }
    }
}
