using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Implementations;
using DataAccessLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsmsApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region DbContext
            services.AddDbContext<GsmsContext>();
            #endregion

            #region Repository
            services.AddScoped(typeof(IGenericRepository<Brand>), typeof(GenericRepository<Brand>));
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<ImportOrder>, GenericRepository<ImportOrder>>();
            services.AddScoped<IGenericRepository<ImportOrderDetail>, GenericRepository<ImportOrderDetail>>();
            services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            services.AddScoped<IGenericRepository<ProductDetail>, GenericRepository<ProductDetail>>();
            services.AddScoped<IGenericRepository<Receipt>, GenericRepository<Receipt>>();
            services.AddScoped<IGenericRepository<ReceiptDetail>, GenericRepository<ReceiptDetail>>();
            services.AddScoped<IGenericRepository<Store>, GenericRepository<Store>>();
            #endregion

            #region UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            return services;
        }
    }
}
