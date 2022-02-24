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

        public async Task<IEnumerable<Store>> GetStoresAsync(
            SortType? sortByName,
            SortType? sortByDate,
            int page,
            int pageSize
            )
        {
            IEnumerable<Store> stores = await work.Stores.GetAllAsync();
            stores = from store in stores
                     where store.IsDeleted == false
                     select store;
            if (sortByName.HasValue)
            {
                stores = GsmsUtils.Sort(stores, s => s.Name, sortByName.Value);
            } else if (sortByDate.HasValue)
            {
                stores = GsmsUtils.Sort(stores, s => s.CreatedDate, sortByDate.Value);
            } else if (!sortByName.HasValue && !sortByDate.HasValue)
            {
                stores = GsmsUtils.Sort(stores, s => s.CreatedDate, SortType.DESC);
            }

            stores = GsmsUtils.Paging(stores, page, pageSize);
            return stores;
        }

        public async Task<Store> GetStoreAsync(string id)
        {
            Store store = await work.Stores.GetAsync(id);
            if (store != null && store.IsDeleted == true)
            {
                return null;
            }
            return store;
        }

        public async Task<IEnumerable<Store>> GetStoresAsync(string brandId)
        {
            IEnumerable<Store> stores = await work.Stores.GetAllAsync();
            stores = from store in stores
                     where store.BrandId.Equals(brandId) && store.IsDeleted == false
                     select store;
            return stores;
        }

        public async Task<Store> AddStoreAsync(Store newStore)
        {
            await CheckStore(newStore);
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
            if (store == null || store.IsDeleted == true)
            {
                throw new Exception("Store is not existed!!");
            }
            await CheckStore(updatedStore);
            store.Name = updatedStore.Name;
            store.IsDeleted = updatedStore.IsDeleted;
            store.BrandId = updatedStore.BrandId;
            work.Stores.Update(store);
            work.Save();
            return store;
        }

        private async Task CheckStore(Store store)
        {
            Brand brand = await work.Brands.GetAsync(store.BrandId);
            if (brand == null)
            {
                throw new Exception("Brand is not existed!!");
            }
        }
        public async Task DeleteStoreAsync(string id)
        {
            Store store = await work.Stores.GetAsync(id);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            //work.Stores.Delete(store);
            store.IsDeleted = true;
            work.Stores.Update(store);
            work.Save();
        }
    }
}
