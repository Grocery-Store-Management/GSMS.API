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
            List<ReceiptDetail> receiptDetails = newReceipt.ReceiptDetails.ToList();
            Store store = await work.Stores.GetAsync(newReceipt.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            newReceipt.Id = GsmsUtils.CreateGuiId();
            newReceipt.CreatedDate = DateTime.Now;
            newReceipt.IsDeleted = false;
            if (newReceipt.ReceiptDetails != null && newReceipt.ReceiptDetails.Count > 0)
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
                    if (receiptDetail.Quantity == productDetail.StoredQuantity)
                    {
                        productDetail.Status = Status.OUT_OF_STOCK;
                    } 
                    else if(productDetail.StoredQuantity - receiptDetail.Quantity > 0 && productDetail.StoredQuantity - receiptDetail.Quantity < 10)
                    {
                        productDetail.Status = Status.ALMOST_OUT_OF_STOCK;
                    }
                    receiptDetail.ReceiptId = newReceipt.Id;
                    receiptDetail.Name = product.Name;
                    receiptDetail.Price = productDetail.Price;
                    await work.ReceiptDetails.AddAsync(receiptDetail);
                    productDetail.StoredQuantity -= receiptDetail.Quantity;
                    productDetail.Product = null;
                    work.ProductDetails.Update(productDetail);
                }
            }
            await work.Receipts.AddAsync(newReceipt);
            await work.Save();
            newReceipt.ReceiptDetails = receiptDetails;
            return newReceipt;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync(
            DateTime? startDate, 
            DateTime? endDate,
            SortType? sortByDate,
            int page,
            int pageSize)
        {
            IEnumerable<Receipt> receipts = await work.Receipts.GetAllAsync();
            receipts = receipts.Where(r => r.IsDeleted == false);
            if (startDate.HasValue && endDate.HasValue)
            {
                receipts = receipts.Where(r => r.CreatedDate >= startDate && r.CreatedDate <= endDate);
            }
            if (sortByDate.HasValue)
            {
                receipts = GsmsUtils.Sort(receipts, r => r.CreatedDate, sortByDate.Value);
            } else if(!sortByDate.HasValue)
            {
                receipts = GsmsUtils.Sort(receipts, r => r.CreatedDate, SortType.DESC);
            }
            receipts = GsmsUtils.Paging(receipts, page, pageSize);
            foreach(Receipt receipt in receipts)
            {
                string sql = "select * from ReceiptDetail where ReceiptId = '" + receipt.Id + "'";
                IEnumerable<ReceiptDetail> details = await work.ReceiptDetails.ExecuteQueryAsync(sql);
                receipt.ReceiptDetails = details.ToList();
            }
            return receipts;
        }

        public async Task<Receipt> GetAsync(string id)
        {
            Receipt receipt = await work.Receipts.GetAsync(id);
            if (receipt != null && receipt.IsDeleted == true)
            {
                return null;
            }
            IEnumerable<ReceiptDetail> receiptDetails = await work.ReceiptDetails.GetAllAsync();
            receipt.ReceiptDetails = receiptDetails.Where(r => r.ReceiptId.Equals(receipt.Id)).ToList();
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
                //receiptDetail.Id = GsmsUtils.CreateGuiId();
                receiptDetail.Name = product.Name;
                receiptDetail.Price = productDetail.Price;

                work.ReceiptDetails.Update(receiptDetail);
            }

            receipt.ReceiptDetails = updatedReceipt.ReceiptDetails;
            work.Receipts.Update(receipt);
            await work.Save();
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
            await work.Save();
        }
    }
}
