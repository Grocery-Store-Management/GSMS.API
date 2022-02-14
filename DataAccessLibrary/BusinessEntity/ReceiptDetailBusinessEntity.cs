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
    public class ReceiptDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ReceiptDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<ReceiptDetail> AddReceiptDetailAsync(ReceiptDetail newReceiptDetail)
        {
            Receipt receipt = await work.Receipts.GetAsync(newReceiptDetail.ReceiptId);
            if(receipt == null)
            {
                throw new Exception("Receipt is not existed!!");
            }
            Product product = await work.Products.GetAsync(newReceiptDetail.ProductId);
            if(product == null)
            {
                throw new Exception("Product is not existed!!");
            }
            newReceiptDetail.Id = GsmsUtils.CreateGuiId();
            await work.ReceiptDetails.AddAsync(newReceiptDetail);
            work.Save();
            return newReceiptDetail;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetReceiptDetailsAsync()
        {
            return await work.ReceiptDetails.GetAllAsync();
        }

        public async Task<ReceiptDetail> GetAsync(string id)
        {
            return await work.ReceiptDetails.GetAsync(id);
        }

        public async Task<IEnumerable<ReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(string receiptId)
        {
            IEnumerable<ReceiptDetail> receiptDetails;

            Receipt receipt = await work.Receipts.GetAsync(receiptId);
            if (receipt == null || receipt.IsDeleted == true)
            {
                throw new Exception("Receipt is not existed!!");
            }
            string sql = "select * from ReceiptDetail where ReceiptId = '" + receiptId + "'";
            receiptDetails = await work.ReceiptDetails.ExecuteQueryAsync(sql);
            return receiptDetails;
        }

        public async Task<ReceiptDetail> UpdateReceiptDetailAsync(ReceiptDetail updatedReceiptDetail)
        {
            Receipt receipt = await work.Receipts.GetAsync(updatedReceiptDetail.ReceiptId);
            if (receipt == null || receipt.IsDeleted == true)
            {
                throw new Exception("Receipt is not existed!!");
            }
            Product product = await work.Products.GetAsync(updatedReceiptDetail.ProductId);
            if (product == null)
            {
                throw new Exception("Product is not existed!!");
            }

            ReceiptDetail receiptDetail = await work.ReceiptDetails.GetAsync(updatedReceiptDetail.Id);
            if (receiptDetail == null)
            {
                throw new Exception("Receipt detail is not existed!!");
            }
            if (updatedReceiptDetail.ProductId != null)
            {
                receiptDetail.ProductId = updatedReceiptDetail.ProductId;
            }
            if (updatedReceiptDetail.Quantity != null)
            {
                receiptDetail.Quantity = updatedReceiptDetail.Quantity;
            }
            work.ReceiptDetails.Update(receiptDetail);
            work.Save();
            return receiptDetail;
        }

        public async Task DeleteReceiptDetailAsync(string id)
        {
            ReceiptDetail receiptDetail = await work.ReceiptDetails.GetAsync(id);
            if (receiptDetail == null)
            {
                throw new Exception("Receipt detail is not existed!!");
            }
            work.ReceiptDetails.Delete(receiptDetail);
            work.Save();
        }

        public async Task DeleteReceiptDetailsByReceiptIdAsync(string receiptId)
        {
            Receipt receipt = await work.Receipts.GetAsync(receiptId);
            if (receipt == null || receipt.IsDeleted == true)
            {
                throw new Exception("Receipt is not existed!!");
            }
            string sql = "select * from ReceiptDetail where ReceiptId = '" + receiptId + "'";
            IEnumerable<ReceiptDetail> listToRemove = await work.ReceiptDetails.ExecuteQueryAsync(sql);
            foreach (ReceiptDetail removedReceiptDetail in listToRemove)
            {
                work.ReceiptDetails.Delete(removedReceiptDetail);
            }
            work.Save();
        }
    }
}
