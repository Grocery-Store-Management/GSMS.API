using BusinessObjectLibrary;
using DataAccessLibrary.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories
        IGenericRepository<Brand> Brands { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        IGenericRepository<ImportOrder> ImportOrders { get; }
        IGenericRepository<ProductDetail> ProductDetails { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ReceiptDetail> ReceiptDetails { get; }
        IGenericRepository<Receipt> Receipts { get; }
        IGenericRepository<Store> Stores { get; }
        #endregion

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        Task<int> Save();
    }
}
