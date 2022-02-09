using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GsmsContext context;

        #region Repositories
        public GenericRepository<Brand> Brands { get; }
        public GenericRepository<Category> Categories { get; }
        public GenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        public GenericRepository<ImportOrder> ImportOrders { get; }
        public GenericRepository<ProductDetail> ProductDetails { get; }
        public GenericRepository<Product> Products { get; }
        public GenericRepository<ReceiptDetail> ReceiptDetails { get; }
        public GenericRepository<Receipt> Receipts { get; }
        public GenericRepository<Store> Stores { get; }
        #endregion

        //#region Database context
        //public GsmsContext Context
        //{
        //    get
        //    {
        //        return this.context;
        //    }
        //}
        //#endregion

        public UnitOfWork(GsmsContext context,
            GenericRepository<Brand> brandRepository, GenericRepository<Category> categoryRepository,
            GenericRepository<ImportOrderDetail> importOrderDetailRepository,
            GenericRepository<ImportOrder> importOrderRepository,
            GenericRepository<ProductDetail> productDetailRepository,
            GenericRepository<Product> productRepository,
            GenericRepository<ReceiptDetail> receiptDetailRepository,
            GenericRepository<Receipt> receiptRepository,
            GenericRepository<Store> storeRepository
            )
        {
            this.context = context;
            this.Brands = brandRepository;
            this.Categories = categoryRepository;
            this.ImportOrderDetails = importOrderDetailRepository;
            this.ImportOrders = importOrderRepository;
            this.ProductDetails = productDetailRepository;
            this.Products = productRepository;
            this.ReceiptDetails = receiptDetailRepository;
            this.Receipts = receiptRepository;
            this.Stores = storeRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
