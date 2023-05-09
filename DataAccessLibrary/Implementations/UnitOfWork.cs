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
        public IGenericRepository<Brand> Brands { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        public IGenericRepository<ImportOrder> ImportOrders { get; }
        public IGenericRepository<ProductDetail> ProductDetails { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<ReceiptDetail> ReceiptDetails { get; }
        public IGenericRepository<Receipt> Receipts { get; }
        public IGenericRepository<Store> Stores { get; }
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
            IGenericRepository<Brand> brandRepository, IGenericRepository<Category> categoryRepository,
            IGenericRepository<ImportOrderDetail> importOrderDetailRepository,
            IGenericRepository<ImportOrder> importOrderRepository,
            IGenericRepository<ProductDetail> productDetailRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<ReceiptDetail> receiptDetailRepository,
            IGenericRepository<Receipt> receiptRepository,
            IGenericRepository<Store> storeRepository
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

        public async Task<int> Save()
        {
            context.SaveChanges();

            //Save Changes to Repo
            //await Brands.SaveChangesToRedis();
            //await Categories.SaveChangesToRedis();
            //await ImportOrders.SaveChangesToRedis();
            //await ImportOrderDetails.SaveChangesToRedis();
            //await Products.SaveChangesToRedis();
            //await ProductDetails.SaveChangesToRedis();
            //await Receipts.SaveChangesToRedis();
            //await ReceiptDetails.SaveChangesToRedis();
            //await Stores.SaveChangesToRedis();

            return 1;
        }
    }
}
