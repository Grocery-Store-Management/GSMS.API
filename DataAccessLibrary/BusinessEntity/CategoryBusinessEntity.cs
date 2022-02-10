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
    public class CategoryBusinessEntity
    {
        private IUnitOfWork work;
        public CategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await work.Categories.GetAllAsync();
        }

        public async Task<Category> GetCategoryAsync(string id)
        {
            return await work.Categories.GetAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            newCategory.Id = GsmsUtils.CreateGuiId();
            newCategory.IsDeleted = false;
            await work.Categories.AddAsync(newCategory);
            work.Save();
            return newCategory;
        }

        public async Task<Category> UpdateCategoryAsync(Category updatedCategory)
        {
            Category category = await work.Categories.GetAsync(updatedCategory.Id);
            if (category == null)
            {
                throw new Exception("Category is not existed!!");
            }
            category.Name = updatedCategory.Name;
            category.IsDeleted = updatedCategory.IsDeleted;
            work.Categories.Update(category);
            work.Save();
            return category;
        }

        public async Task DeleteCategoryAsync(string id)
        {
            Category category = await work.Categories.GetAsync(id);
            if (category == null)
            {
                throw new Exception("Category is not existed!!");
            }
            work.Categories.Delete(category);
            work.Save();
        }
    }
}
