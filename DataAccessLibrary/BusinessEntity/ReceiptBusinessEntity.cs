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
    public class ReceiptBusinessEntity
    {
        private IUnitOfWork work; 
        public ReceiptBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<Receipt> AddReceiptAsync(Receipt newReceipt)
        {
            Store store = await work.Stores.GetAsync(newReceipt.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            newReceipt.Id = GsmsUtils.CreateGuiId();
            newReceipt.CreatedDate = DateTime.Now;
            newReceipt.IsDeleted = false;
            await work.Receipts.AddAsync(newReceipt);
            work.Save();
            return newReceipt;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync()
        {
            return await work.Receipts.GetAllAsync();
        }

        public async Task<Receipt> GetAsync(string id)
        {
            return await work.Receipts.GetAsync(id);
        }

        public async Task<Receipt> UpdateReceiptAsync(Receipt updatedReceipt)
        {
            Store store = await work.Stores.GetAsync(updatedReceipt.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }

            Receipt receipt = await work.Receipts.GetAsync(updatedReceipt.Id);
            if (receipt == null)
            {
                throw new Exception("Receipt is not existed!!");
            }
            if (updatedReceipt.CustomerId != null)
            {
                receipt.CustomerId = updatedReceipt.CustomerId;
            }
            if (updatedReceipt.EmployeeId != null)
            {
                receipt.EmployeeId = updatedReceipt.EmployeeId;
            }
            if (updatedReceipt.StoreId != null)
            {
                receipt.StoreId = updatedReceipt.StoreId;
            }
            if (updatedReceipt.IsDeleted != null)
            {
                receipt.IsDeleted = updatedReceipt.IsDeleted;
            }
            work.Receipts.Update(receipt);
            work.Save();
            return receipt;
        }

        public async Task DeleteReceiptAsync(string id)
        {
            Receipt receipt = await work.Receipts.GetAsync(id);
            if (receipt == null)
            {
                throw new Exception("Receipt is not existed!!");
            }
            work.Receipts.Delete(receipt);
            work.Save();
        }
    }
}
