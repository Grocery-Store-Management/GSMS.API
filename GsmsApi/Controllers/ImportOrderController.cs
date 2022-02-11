﻿using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GsmsApi.Controllers
{
    //PhucVVT
    [Route("api/[controller]")]
    [ApiController]
    public class ImportOrderController : ControllerBase
    {
        private ImportOrderBusinessEntity importOrderEntity;

        public ImportOrderController(IUnitOfWork work)
        {
            importOrderEntity = new ImportOrderBusinessEntity(work);
        }

        // GET: api/<ImportOrderController>
        /// <summary>
        /// Get All the import orders
        /// </summary>
        /// <returns>List of import orders</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<ImportOrder>), 200)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<ImportOrder> importOrders = await importOrderEntity.GetImportOrdersAsync();
                return StatusCode(200, importOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ImportOrderController>/5
        /// <summary>
        /// Get import order based on ID
        /// </summary>
        /// <param name="id">The ID string of the import order</param>
        /// <returns>The existed import order</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ImportOrder), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                ImportOrder importOrder = await importOrderEntity.GetAsync(id);
                if (importOrder == null)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, importOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // POST api/<ImportOrderController>
        /// <summary>
        /// Add a new import order
        /// </summary>
        /// <param name="newImportOrder">New import order to be added, NAME property of the import order must be provided</param>
        /// <returns>The added import order</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ImportOrder), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody] ImportOrder newImportOrder)
        {
            try
            {
                if (string.IsNullOrEmpty(newImportOrder.Name))
                {
                    throw new Exception("Import order name is empty!!");
                }
                ImportOrder addedImportOrder = await importOrderEntity.AddImportOrderAsync(newImportOrder);
                return StatusCode(201, addedImportOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //PUT api/<ImportOrderController>/5
        /// <summary>
        /// Update import order
        /// </summary>
        /// <param name="id">ID of the import order to be updated</param>
        /// <param name="updatedImportOrder">New information to update the import order</param>
        /// <returns>The updated import order</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ImportOrder), 201)]
        public async Task<IActionResult> Put(string id, [FromBody] ImportOrder updatedImportOrder)
        {
            try
            {
                if (!id.Equals(updatedImportOrder.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                ImportOrder importOrder = await importOrderEntity.UpdateImportOrderAsync(updatedImportOrder);
                return StatusCode(200, importOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ImportOrderController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await importOrderEntity.DeleteImportOrderAsync(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}