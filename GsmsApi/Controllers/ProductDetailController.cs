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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/product-details")]
    public class ProductDetailController : ControllerBase
    {
        private ProductDetailBusinessEntity productDetailEntity;
        public ProductDetailController(IUnitOfWork work)
        {
            productDetailEntity = new ProductDetailBusinessEntity(work);
        }

        // GET: api/<ProductDetailController>
        /// <summary>
        /// Get All the Product Details. Product Details can be filterd by Product or Status
        /// </summary>
        /// <param name="productId">Product ID to filter</param>
        /// <param name="status">Status to filter. 1 for Active, -1 for Inactive</param>
        /// <param name="sortByPrice">Sort by Price</param>
        /// <param name="sortByStoredQuantity">Sort by Stored Quantity</param>
        /// <param name="sortByManufacturingDate">Sort by Manufaturing Date</param>
        /// <param name="sortByExpiringDate">Sort by Expiring Date</param>
        /// <param name="page">Page number, 0 to get all</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of products</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), 200)]
        public async Task<IActionResult> GetsAsync(
            [FromQuery(Name = "product-id")] string productId, 
            [FromQuery] Status? status,
            [FromQuery(Name = "sort-by-price")] SortType? sortByPrice,
            [FromQuery(Name = "sort-by-stored-quantity")] SortType? sortByStoredQuantity,
            [FromQuery(Name = "sort-by-manufaturing-date")] SortType? sortByManufacturingDate,
            [FromQuery(Name = "sort-by-expiring-date")] SortType? sortByExpiringDate,
            [FromQuery] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10)
        {
            IEnumerable<ProductDetail> products = new List<ProductDetail>();
            try
            {
                if (string.IsNullOrEmpty(productId) && status == null)
                {
                    products = await productDetailEntity.GetProductDetailsAsync(sortByPrice, sortByStoredQuantity, sortByManufacturingDate, sortByExpiringDate, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(productId))
                {
                    products = await productDetailEntity.GetProductDetailsAsync(productId);
                }
                else if (status != null)
                {
                    products = await productDetailEntity.GetProductDetailsAsync(status.Value);
                }
                else
                {
                    // Filter both
                }
                return StatusCode(200, products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProductDetailController>/5
        /// <summary>
        /// Get Product Detail based on ID
        /// </summary>
        /// <param name="id">The ID string of the Product Detail</param>
        /// <returns>The existed Product Detail</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProductDetail), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                ProductDetail productDetail = await productDetailEntity.GetProductDetailAsync(id);
                if (productDetail == null)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, productDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProductDetailController>
        /// <summary>
        /// Add a new ProductDetail
        /// </summary>
        /// <param name="newProductDetail">New ProductDetail to be added, PRODUCTID, PRICE and STOREDQUANTITY property of productDetail must be provided</param>
        /// <returns>The added ProductDetail</returns>
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ProductDetail), 201)]
        public async Task<IActionResult> PostAsync([FromBody] ProductDetail newProductDetail)
        {
            try
            {
                ProductDetail addedProductDetail = await productDetailEntity.AddProductDetailAsync(newProductDetail);
                return StatusCode(201, addedProductDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProductDetailController>/5
        /// <summary>
        /// Update ProductDetail
        /// </summary>
        /// <param name="id">ID of the ProductDetail to be updated</param>
        /// <param name="updatedProductDetail">New information to update</param>
        /// <returns>The updated productDetail</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ProductDetail), 201)]
        public async Task<IActionResult> PutAsync(string id, [FromBody] ProductDetail updatedProductDetail)
        {
            try
            {
                if (!id.Equals(updatedProductDetail.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                ProductDetail productDetail = await productDetailEntity.UpdateProductDetailAsync(updatedProductDetail);
                return StatusCode(201, productDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*
        // DELETE api/<ProductDetailController>/5
        /// <summary>
        /// Delete Product Detail
        /// </summary>
        /// <param name="id">ID string of product detail to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await productDetailEntity.DeleteProductDetailAsync(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return
                    StatusCode(500, ex.Message);
            }
        }
        */
    }
    }
