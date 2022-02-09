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
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private BrandBusinessEntity brandEntity;
        //private IUnitOfWork work;
        public BrandController(IUnitOfWork work)
        {
            brandEntity = new BrandBusinessEntity(work);
        }

        // GET: api/<BrandController>
        /// <summary>
        /// Get All the brands
        /// </summary>
        /// <returns>List of brands</returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<Brand> brands = await brandEntity.GetBrandsAsync();
            return StatusCode(200, brands);
        }

        // GET api/<BrandController>/5
        /// <summary>
        /// Get brand based on ID
        /// </summary>
        /// <param name="id">The ID string of the brand</param>
        /// <returns>The existed brand</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            Brand brand = await brandEntity.GetAsync(id);
            return StatusCode(200, brand);
        }

        // POST api/<BrandController>
        /// <summary>
        /// Add new brand
        /// </summary>
        /// <param name="brandName">Name of the brand</param>
        /// <returns>The added brand</returns>
        [ProducesResponseType(typeof(Brand), 200)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Brand newBrand)
        {
            Brand addedBrand = await brandEntity.AddBrandAsync(newBrand);
            return StatusCode(200, addedBrand);
        }

        // PUT api/<BrandController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<BrandController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
