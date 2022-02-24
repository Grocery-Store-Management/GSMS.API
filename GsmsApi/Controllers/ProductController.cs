using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GsmsApi.Controllers
{
    //PhongNT
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/products")]
    public class ProductController : ControllerBase
    {
        private ProductBusinessEntity productEntity;
        public ProductController(IUnitOfWork work)
        {
            productEntity = new ProductBusinessEntity(work);
        }

        // GET: api/<ProductController>
        /// <summary>
        /// Get All the Products. Products can be filterd by Category or MasterProduct
        /// </summary>
        /// <param name="categoryId">Category ID to filter</param>
        /// <param name="masterProductId">Master Product ID to filter</param>
        /// <param name="sortByName">Sort by Product Name</param>
        /// <param name="page">Page number, 0 to get all</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of products</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<IActionResult> GetsAsync(
            [FromQuery] string categoryId, 
            [FromQuery] string masterProductId,
            [FromQuery] SortType? sortByName,
            int page = 1,
            int pageSize = 10
            )
        {
            IEnumerable<Product> products = new List<Product>();
            try
            {
                if (string.IsNullOrEmpty(categoryId) && string.IsNullOrEmpty(masterProductId))
                {
                    products = await productEntity.GetProductsAsync(sortByName, page, pageSize);
                } else if (!string.IsNullOrEmpty(categoryId))
                {
                    products = await productEntity.GetProductsByCategoryAsync(categoryId);
                } else if (!string.IsNullOrEmpty(masterProductId))
                {
                    products = await productEntity.GetProductsByMasterProductAsync(masterProductId);
                } else
                {
                    // Filter both
                }
                return StatusCode(200, products);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProductController>/5
        /// <summary>
        /// Get product based on ID
        /// </summary>
        /// <param name="id">The ID string of the product</param>
        /// <returns>The existed product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Product), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                Product product = await productEntity.GetProductAsync(id);
                return StatusCode(200, product);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProductController>
        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="newProduct">New product to be added, NAME, CATEGORYID and ATOMICPRICE property of product must be provided</param>
        /// <returns>The added Product</returns>
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Product), 201)]
        public async Task<IActionResult> PostAsync([FromBody] Product newProduct)
        {
            try
            {
                if (string.IsNullOrEmpty(newProduct.Name))
                {
                    throw new Exception("Product name is empty!!");
                }
                if (string.IsNullOrEmpty(newProduct.CategoryId))
                {
                    throw new Exception("Category ID is empty!!");
                }
                Product addedProduct = await productEntity.AddProductAsync(newProduct);
                return StatusCode(201, addedProduct);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProductController>/5
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id">ID of the product to be updated</param>
        /// <param name="updatedProduct">New information to update</param>
        /// <returns>The updated product</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Product), 201)]
        public async Task<IActionResult> PutAsync(string id, [FromBody] Product updatedProduct)
        {
            try
            {
                if (!id.Equals(updatedProduct.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                Product product = await productEntity.UpdateProductAsync(updatedProduct);
                return StatusCode(201, product);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProductController>/5
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id">ID string of the product to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await productEntity.DeleteProductAsync(id);
                return StatusCode(200);
            } catch (Exception ex)
            {
                return
                    StatusCode(500, ex.Message);
            }
        }
    }
}
