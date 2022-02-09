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
    public class BrandBusinessEntity
    {
        private IUnitOfWork work;
        public BrandBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<Brand> AddBrandAsync(Brand newBrand)
        {
            newBrand.Id = GsmsUtils.CreateGuiId();
            newBrand.CreatedDate = DateTime.Now;
            newBrand.IsDeleted = false;
            await work.Brands.AddAsync(newBrand);
            work.Save();
            return newBrand;
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            return await work.Brands.GetAllAsync();
        }

        public async Task<Brand> GetAsync(string id)
        {
            return await work.Brands.GetAsync(id);
        }
    }
}
