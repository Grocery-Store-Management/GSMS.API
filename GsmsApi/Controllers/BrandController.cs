﻿using BusinessObjectLibrary;
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
    public class BrandController : ControllerBase
    {
        private BrandBusinessEntity brandEntity;
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
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Brand>), 200)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<Brand> brands = await brandEntity.GetBrandsAsync();
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
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                Brand brand = await brandEntity.GetAsync(id);
                if (brand == null)
                {
                    return StatusCode(204);
                }
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
        public async Task<IActionResult> Put(string id, [FromBody] Brand updatedBrand)
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

        // DELETE api/<BrandController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await brandEntity.DeleteBrandAsync(id);
                return StatusCode(204);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
