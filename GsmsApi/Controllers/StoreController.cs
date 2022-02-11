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
    public class StoreController : ControllerBase
    {
        private StoreBusinessEntity storeEntity;
        public StoreController(IUnitOfWork work)
        {
            storeEntity = new StoreBusinessEntity(work);
        }

        // GET: api/<StoreController>?brandId=abc
        /// <summary>
        /// Get All the stores, the brandId parameter is optional. Provide the brandId in case you want to 
        /// filter stores by brand
        /// </summary>
        /// <returns>List of stores</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Store>), 200)]
        public async Task<IActionResult> GetAsync([FromQuery] string? brandId)
        {
            IEnumerable<Store> stores;
            try
            {
                if (string.IsNullOrEmpty(brandId))
                {
                    stores = await storeEntity.GetStoresAsync();
                }
                else
                {
                    stores = await storeEntity.GetStoresAsync(brandId);
                }
                return StatusCode(200, stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<StoreController>/5
        /// <summary>
        /// Get store based on ID
        /// </summary>
        /// <param name="id">The ID string of the store</param>
        /// <returns>The existed store</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Store), 200)]
        public async Task<IActionResult> GetsAsync(string id)
        {
            try
            {
                Store store = await storeEntity.GetStoreAsync(id);
                if (store == null)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, store);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add a new store
        /// </summary>
        /// <param name="newStore">New store to be added, NAME and BRANDID property of the store must be provided</param>
        /// <returns>The added store</returns>
        // POST api/<StoreController>
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Store), 201)]
        public async Task<IActionResult> PostAsync([FromBody] Store newStore)
        {
            try
            {
                if (string.IsNullOrEmpty(newStore.Name))
                {
                    throw new Exception("Store name is empty!!");
                }
                if (string.IsNullOrEmpty(newStore.BrandId))
                {
                    throw new Exception("Brand Id is empty!!");
                }
                Store addedStore = await storeEntity.AddStoreAsync(newStore);
                return StatusCode(201, addedStore);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<StoreController>/5
        /// <summary>
        /// Update store
        /// </summary>
        /// <param name="id">ID of the store to be updated</param>
        /// <param name="updatedStore">New information to update</param>
        /// <returns>The updated store</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Store), 201)]
        public async Task<IActionResult> Put(string id, [FromBody] Store updatedStore)
        {
            try
            {
                if (!id.Equals(updatedStore.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                Store store = await storeEntity.UpdateStoreAsync(updatedStore);
                return StatusCode(201, store);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete a store
        /// </summary>
        /// <param name="id">ID of the store to be deleted</param>
        /// <returns></returns>
        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await storeEntity.DeleteStoreAsync(id);
                return StatusCode(204);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}