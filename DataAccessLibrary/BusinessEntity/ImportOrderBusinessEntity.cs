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
            newImportOrder.CreatedDate = DateTime.Now;
            newImportOrder.IsDeleted = false;
            if (newImportOrder.ImportOrderDetails != null && newImportOrder.ImportOrderDetails.Count > 0)
            {
                foreach (ImportOrderDetail importOrderDetail in newImportOrder.ImportOrderDetails)
                {
                    importOrderDetail.Id = GsmsUtils.CreateGuiId();
                    Product product = await work.Products.GetAsync(importOrderDetail.ProductId);
                    IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
                    ProductDetail productDetail = productDetails.Where(p => p.ProductId == product.Id).FirstOrDefault();
                    if (product == null || product.IsDeleted == true)
                    {
                        throw new Exception("Product is not existed!!");
                    }
                    importOrderDetail.ImportOrderId = newImportOrder.Id;
                    importOrderDetail.Name = product.Name;
                    //importOrderDetail.Price = productDetail.Price;
                    await work.ImportOrderDetails.AddAsync(importOrderDetail);
                    productDetail.StoredQuantity += importOrderDetail.Quantity;
                    productDetail.Product = null;
                    work.ProductDetails.Update(productDetail);
                }
            }
            await work.ImportOrders.AddAsync(newImportOrder);
            await work.Save();
            return newImportOrder;
        }

        public async Task<IEnumerable<ImportOrder>> GetImportOrdersAsync(
            DateTime? startDate, 
            DateTime? endDate,
            string? searchByName,
            SortType? sortByName,
            SortType? sortByDate,
            int page,
            int pageSize)
        {
            IEnumerable<ImportOrder> importOrders = await work.ImportOrders.GetAllAsync();
            importOrders = importOrders.Where(i => i.IsDeleted == false);
            if (startDate.HasValue && endDate.HasValue)
            {
                importOrders = importOrders.Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate);
            }
            if (!string.IsNullOrEmpty(searchByName))
            {
                importOrders = importOrders.Where(i => i.Name.ToLower().Contains(searchByName.Trim().ToLower()));
            }
            if (sortByName.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.Name, sortByName.Value);
            } else if(sortByDate.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.CreatedDate, sortByDate.Value);
            } else if(!sortByName.HasValue && !sortByDate.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.CreatedDate, SortType.DESC);
            }
            importOrders = GsmsUtils.Paging(importOrders, page, pageSize);
            foreach (ImportOrder importOrder in importOrders)
            {
                string sql = "select * from ImportOrderDetail where ImportOrderId = '" + importOrder.Id + "'";
                IEnumerable<ImportOrderDetail> details = await work.ImportOrderDetails.ExecuteQueryAsync(sql);
                importOrder.ImportOrderDetails = details.ToList();
            }
            return importOrders;
        }

        public async Task<ImportOrder> GetAsync(string id)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(id);
            if(importOrder != null &&  importOrder.IsDeleted == true)
            {
                return null;
            }
            return importOrder;
        }

        public async Task<ImportOrder> UpdateImportOrderAsync(ImportOrder updatedImportOrder)
        {
            Store store = await work.Stores.GetAsync(updatedImportOrder.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }

            ImportOrder importOrder = await work.ImportOrders.GetAsync(updatedImportOrder.Id);
            if(importOrder == null || importOrder.IsDeleted == true)
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
            importOrder.ImportOrderDetails = updatedImportOrder.ImportOrderDetails;
            work.ImportOrders.Update(importOrder);
            await work.Save();
            return importOrder;
        }

        public async Task DeleteImportOrderAsync(string id)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(id);
            if(importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            //work.ImportOrders.Delete(importOrder);
            importOrder.IsDeleted = true;
            work.ImportOrders.Update(importOrder);
            await work.Save();
        }
    }
}
