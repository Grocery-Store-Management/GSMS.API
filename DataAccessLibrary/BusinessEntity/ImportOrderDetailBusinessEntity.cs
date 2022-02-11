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
    public class ImportOrderDetailBusinessEntity
    {
        private IUnitOfWork work; 
        public ImportOrderDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<ImportOrderDetail> AddImportOrderDetailAsync(ImportOrderDetail newImportOrderDetail)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(newImportOrderDetail.OrderId);
            if(importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            Product product = await work.Products.GetAsync(newImportOrderDetail.ProductId);
            if (product == null)
            {
                throw new Exception("Product is not existed!!");
            }
            newImportOrderDetail.Id = GsmsUtils.CreateGuiId();
            newImportOrderDetail.IsDeleted = false;
            await work.ImportOrderDetails.AddAsync(newImportOrderDetail);
            work.Save();
            return newImportOrderDetail;
        }

        public async Task<IEnumerable<ImportOrderDetail>> GetImportOrderDetailsAsync()
        {
            return await work.ImportOrderDetails.GetAllAsync();
        }

        public async Task<ImportOrderDetail> GetAsync(string id)
        {
            return await work.ImportOrderDetails.GetAsync(id);
        }

        public async Task<IEnumerable<ImportOrderDetail>> GetImportOrderDetailsByImportOrderIdAsync(string importOrderId)
        {
            IEnumerable<ImportOrderDetail> importOrderDetails;

            ImportOrder importOrder = await work.ImportOrders.GetAsync(importOrderId);
            if (importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            string sql = "select * from ImportOrderDetail where OrderId = '" + importOrderId + "'";
            importOrderDetails = await work.ImportOrderDetails.ExecuteQueryAsync(sql);
            return importOrderDetails;
        }

        public async Task<ImportOrderDetail> UpdateImportOrderDetailAsync(ImportOrderDetail updatedImportOrderDetail)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(updatedImportOrderDetail.OrderId);
            if (importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            Product product = await work.Products.GetAsync(updatedImportOrderDetail.ProductId);
            if (product == null)
            {
                throw new Exception("Product is not existed!!");
            }

            ImportOrderDetail importOrderDetail = await work.ImportOrderDetails.GetAsync(updatedImportOrderDetail.Id);
            if(importOrderDetail == null)
            {
                throw new Exception("Import order detail is not existed!!");
            }
            if(updatedImportOrderDetail.OrderId != null)
            {
                importOrderDetail.OrderId = updatedImportOrderDetail.OrderId;
            }
            if (updatedImportOrderDetail.Name != null)
            {
                importOrderDetail.Name = updatedImportOrderDetail.Name;
            }
            if (updatedImportOrderDetail.Distributor != null)
            {
                importOrderDetail.Distributor = updatedImportOrderDetail.Distributor;
            }
            if (updatedImportOrderDetail.ProductId != null)
            {
                importOrderDetail.ProductId = updatedImportOrderDetail.ProductId;
            }
            if (updatedImportOrderDetail.Quantity != null)
            {
                importOrderDetail.Quantity = updatedImportOrderDetail.Quantity;
            }
            if (updatedImportOrderDetail.IsDeleted != null)
            {
                importOrderDetail.IsDeleted = updatedImportOrderDetail.IsDeleted;
            }
            if (updatedImportOrderDetail.ManufacturingDate != null)
            {
                importOrderDetail.ManufacturingDate = updatedImportOrderDetail.ManufacturingDate;
            }
            if (updatedImportOrderDetail.ExpiringDate != null)
            {
                importOrderDetail.ExpiringDate = updatedImportOrderDetail.ExpiringDate;
            }
            work.ImportOrderDetails.Update(importOrderDetail);
            work.Save();
            return importOrderDetail;
        }

        public async Task DeleteImportOrderDetailAsync(string id)
        {
            ImportOrderDetail importOrderDetail = await work.ImportOrderDetails.GetAsync(id);
            if(importOrderDetail == null)
            {
                throw new Exception("Import order detail is not existed!!");
            }
            work.ImportOrderDetails.Delete(importOrderDetail);
            work.Save();
        }

        public async Task DeleteImportOrderDetailsByImportOrderIdAsync(string importOrderId)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(importOrderId);
            if (importOrder == null)
            {
                throw new Exception("Import order is not existed!!");
            }
            string sql = "select * from ImportOrderDetail where OrderId = '" + importOrderId + "'";
            IEnumerable<ImportOrderDetail> listToRemove = await work.ImportOrderDetails.ExecuteQueryAsync(sql);
            foreach(ImportOrderDetail removedImportOrderDetail in listToRemove)
            {
                work.ImportOrderDetails.Delete(removedImportOrderDetail);
            }
            work.Save();
        }
    }
}
