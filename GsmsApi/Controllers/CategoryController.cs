using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GsmsApi.Controllers
{
    //PhongNT
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryBusinessEntity categoryEntity;
        public CategoryController(IUnitOfWork work)
        {
            categoryEntity = new CategoryBusinessEntity(work);
        }

        // GET: api/<CategoryController>
        /// <summary>
        /// Get All the Categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<Category> categories = await categoryEntity.GetCategoriesAsync();
                return StatusCode(200, categories);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get category based on ID
        /// </summary>
        /// <param name="id">The ID string of the category</param>
        /// <returns>The existed category</returns>
        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Category), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                Category category = await categoryEntity.GetCategoryAsync(id);
                return StatusCode(200, category);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<CategoryController>
        /// <summary>
        /// Add a new Category
        /// </summary>
        /// <param name="newCategory">New category to be added, NAME property of the category must be provided</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Category), 201)]
        public async Task<IActionResult> PostAsync([FromBody] Category newCategory)
        {
            try
            {
                if (string.IsNullOrEmpty(newCategory.Name))
                {
                    throw new Exception("Category name is empty!!");
                }
                Category addedCategory = await categoryEntity.AddCategoryAsync(newCategory);
                return StatusCode(201, addedCategory);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id">ID of the category to be updated</param>
        /// <param name="updatedCategory">New information to update</param>
        /// <returns>The updated category</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Category), 201)]
        public async Task<IActionResult> PutAsync(string id, [FromBody] Category updatedCategory)
        {
            try
            {
                if (!id.Equals(updatedCategory.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                Category category = await categoryEntity.UpdateCategoryAsync(updatedCategory);
                return StatusCode(201, category);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await categoryEntity.DeleteCategoryAsync(id);
                return StatusCode(200);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
