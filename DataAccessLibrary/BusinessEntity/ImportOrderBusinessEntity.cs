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
    public class ImportOrderBusinessEntity
    {
        private IUnitOfWork work; 
        public ImportOrderBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<ImportOrder> AddImportOrderAsync(ImportOrder newImportOrder)
        {
            Store store = await work.Stores.GetAsync(newImportOrder.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            newImportOrder.Id = GsmsUtils.CreateGuiId();
            newImportOrder.IsDeleted = false;
            await work.ImportOrders.AddAsync(newImportOrder);
            work.Save();
            return newImportOrder;
        }

        public async Task<IEnumerable<ImportOrder>> GetImportOrdersAsync()
        {
            return await work.ImportOrders.GetAllAsync();
        }

        public async Task<ImportOrder> GetAsync(string id)
        {
            return await work.ImportOrders.GetAsync(id);
        }

        public async Task<ImportOrder> UpdateImportOrderAsync(ImportOrder updatedImportOrder)
        {
            Store store = await work.Stores.GetAsync(updatedImportOrder.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }

            ImportOrder importOrder = await work.ImportOrders.GetAsync(updatedImportOrder.Id);
            if(importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            if(updatedImportOrder.Name != null)
            {
                importOrder.Name = updatedImportOrder.Name;
            }
            if (updatedImportOrder.ProductId != null)
            {
                importOrder.ProductId = updatedImportOrder.ProductId;
            }
            if(updatedImportOrder.StoreId != null)
            {
                importOrder.StoreId = updatedImportOrder.StoreId;
            }
            if (updatedImportOrder.IsDeleted != null)
            {
                importOrder.IsDeleted = updatedImportOrder.IsDeleted;
            }
            work.ImportOrders.Update(importOrder);
            work.Save();
            return importOrder;
        }

        public async Task DeleteImportOrderAsync(string id)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(id);
            if(importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            work.ImportOrders.Delete(importOrder);
            work.Save();
        }
    }
}
