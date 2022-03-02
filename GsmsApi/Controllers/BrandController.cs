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
    [Route("api/v1.0/brands")]
    public class BrandController : ControllerBase
    {
        private BrandBusinessEntity brandEntity;
        public BrandController(IUnitOfWork work)
        {
            brandEntity = new BrandBusinessEntity(work);
        }

        // GET: api/<BrandController>
        /// <summary>
        /// Get All Brands
        /// </summary>
        /// <param name="searchByName">Search Brand by name</param>
        /// <param name="sortByName">Sort by Brand Name</param>
        /// <param name="sortByDate">Sort by Brand Created Date</param>
        /// <param name="page">Page number, 0 to get all</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of Brands</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Brand>), 200)]
        public async Task<IActionResult> GetAsync(
            [FromQuery(Name = "search-by-name")] string searchByName,
            [FromQuery(Name = "sort-by-name")] SortType? sortByName, 
            [FromQuery(Name = "sort-by-date")] SortType? sortByDate,
            [FromQuery] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10
            )
        {
            try
            {
                IEnumerable<Brand> brands = await brandEntity.GetBrandsAsync(searchByName, sortByName, sortByDate, page, pageSize);
                return StatusCode(200, brands);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<BrandController>/5
        /// <summary>
        /// Get brand based on ID
        /// </summary>
        /// <param name="id">The ID string of the brand</param>
        /// <returns>The existed brand</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Brand), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                Brand brand = await brandEntity.GetBrandAsync(id);
                return StatusCode(200, brand);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // POST api/<BrandController>
        /// <summary>
        /// Add a new brand
        /// </summary>
        /// <param name="newBrand">New brand to be added, NAME property of the brand must be provided</param>
        /// <returns>The added brand</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Brand), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody] Brand newBrand)
        {
            try
            {
                if (string.IsNullOrEmpty(newBrand.Name))
                {
                    throw new Exception("Brand name is empty!!");
                }
                Brand addedBrand = await brandEntity.AddBrandAsync(newBrand);
                return StatusCode(201, addedBrand);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //PUT api/<BrandController>/5
        /// <summary>
        /// Update brand
        /// </summary>
        /// <param name="id">ID of the brand to be updated</param>
        /// <param name="updatedBrand">New information to update the brand</param>
        /// <returns>The updated brand</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Brand), 201)]
        public async Task<IActionResult> PutAsync(string id, [FromBody] Brand updatedBrand)
        {
            try
            {
                if (!id.Equals(updatedBrand.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                Brand brand = await brandEntity.UpdateBrandAsync(updatedBrand);
                return StatusCode(201, brand);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete a brand
        /// </summary>
        /// <param name="id">ID of the brand to be deleted</param>
        /// <returns></returns>
        // DELETE api/<BrandController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await brandEntity.DeleteBrandAsync(id);
                return StatusCode(200);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
