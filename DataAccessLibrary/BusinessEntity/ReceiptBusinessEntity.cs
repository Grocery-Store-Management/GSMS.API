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
            if (newReceipt.ReceiptDetails.Count > 0)
            {
                foreach (ReceiptDetail receiptDetail in newReceipt.ReceiptDetails)
                {
                    receiptDetail.Id = GsmsUtils.CreateGuiId();
                    Product product = await work.Products.GetAsync(receiptDetail.ProductId);
                    IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
                    ProductDetail productDetail = productDetails.Where(p => p.ProductId == product.Id).FirstOrDefault();
                    if (product == null || product.IsDeleted == true)
                    {
                        throw new Exception("Product is not existed!!");
                    }
                    if(receiptDetail.Quantity > productDetail.StoredQuantity)
                    {
                        throw new Exception("Purchase quantity exceeds quantity in stock!!");
                    }
                    receiptDetail.ReceiptId = newReceipt.Id;
                    await work.ReceiptDetails.AddAsync(receiptDetail);
                    productDetail.StoredQuantity -= receiptDetail.Quantity;
                    work.ProductDetails.Update(productDetail);
                }
            }
            await work.Receipts.AddAsync(newReceipt);
            work.Save();
            return newReceipt;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Receipt> receipts = await work.Receipts.GetAllAsync();
            receipts = receipts.Where(r => r.IsDeleted == false);
            if (startDate.HasValue && endDate.HasValue)
            {
                receipts = receipts.Where(r => r.CreatedDate >= startDate || r.CreatedDate <= endDate);
            }
            return receipts.OrderBy(r => r.CreatedDate);
        }

        public async Task<Receipt> GetAsync(string id)
        {
            Receipt receipt = await work.Receipts.GetAsync(id);
            if (receipt != null && receipt.IsDeleted == true)
            {
                return null;
            }
            return receipt;
        }

        public async Task<Receipt> UpdateReceiptAsync(Receipt updatedReceipt)
        {
            Store store = await work.Stores.GetAsync(updatedReceipt.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }

            Receipt receipt = await work.Receipts.GetAsync(updatedReceipt.Id);
            if (receipt == null || receipt.IsDeleted == true)
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
            foreach (ReceiptDetail receiptDetail in updatedReceipt.ReceiptDetails)
            {
                Product product = await work.Products.GetAsync(receiptDetail.ProductId);
                IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
                ProductDetail productDetail = productDetails.Where(p => p.ProductId == product.Id).FirstOrDefault();
                if (product == null || product.IsDeleted == true)
                {
                    throw new Exception("Product is not existed!!");
                }
                if (receiptDetail.Quantity > productDetail.StoredQuantity)
                {
                    throw new Exception("Purchase quantity exceeds quantity in stock!!");
                }
            }
            receipt.ReceiptDetails = updatedReceipt.ReceiptDetails;
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
            //work.Receipts.Delete(receipt);
            receipt.IsDeleted = true;
            work.Receipts.Update(receipt);
            work.Save();
        }
    }
}
