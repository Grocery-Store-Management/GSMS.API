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

        public async Task<IEnumerable<Category>> GetCategoriesAsync(
            SortType? sortByName,
            int page,
            int pageSize)
        {
            IEnumerable<Category> categories = await work.Categories.GetAllAsync();
            categories = from category in categories
                         where category.IsDeleted == false
                         select category;

            if (sortByName.HasValue)
            {
                categories = GsmsUtils.Sort(categories, c => c.Name, sortByName.Value);
            }

            categories = GsmsUtils.Paging(categories, page, pageSize);

            return categories;
        }

        public async Task<Category> GetCategoryAsync(string id)
        {
            Category category = await work.Categories.GetAsync(id);
            if (category != null && category.IsDeleted == true)
            {
                return null;
            }
            return category;
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
            if (category == null || category.IsDeleted == true)
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
            //work.Categories.Delete(category);
            category.IsDeleted = true;
            work.Categories.Update(category);
            work.Save();
        }
    }
}
