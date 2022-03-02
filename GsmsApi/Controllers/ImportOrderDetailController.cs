using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsmsApi.Controllers
{
    //PhucVVT
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/import-order-details")]
    public class ImportOrderDetailController : ControllerBase
    {
        private ImportOrderDetailBusinessEntity importOrderDetailEntity;
        public ImportOrderDetailController(IUnitOfWork work)
        {
            importOrderDetailEntity = new ImportOrderDetailBusinessEntity(work);
        }

        // GET: api/<ImportOrderDetailController>
        /// <summary>
        /// Get All the import order details
        /// </summary>
        /// <returns>List of import order details</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<ImportOrderDetail>), 200)]
        public async Task<IActionResult> GetsAsync([FromQuery] string importOrderId)
        {
            try
            {
                IEnumerable<ImportOrderDetail> importOrderDetails;
                if (string.IsNullOrEmpty(importOrderId))
                {
                    importOrderDetails = await importOrderDetailEntity.GetImportOrderDetailsAsync();
                } else
                {
                    importOrderDetails = await importOrderDetailEntity.GetImportOrderDetailsByImportOrderIdAsync(importOrderId);
                }
                return StatusCode(200, importOrderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ImportOrderDetailController>/5
        /// <summary>
        /// Get import order detail based on ID
        /// </summary>
        /// <param name="id">The ID string of the import order detail</param>
        /// <returns>The existed import order detail</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ImportOrderDetail), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                ImportOrderDetail importOrderDetail = await importOrderDetailEntity.GetAsync(id);
                return StatusCode(200, importOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } 

        // POST api/<ImportOrderDetailController>
        /// <summary>
        /// Add a new import order detail
        /// </summary>
        /// <param name="newImportOrderDetail">New import order detail to be added, NAME property of the import order detail must be provided</param>
        /// <returns>The added import order detail</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ImportOrderDetail), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody] ImportOrderDetail newImportOrderDetail)
        {
            try
            {
                if (string.IsNullOrEmpty(newImportOrderDetail.Name))
                {
                    throw new Exception("Import order detail name is empty!!");
                }
                ImportOrderDetail addedImportOrderDetail = await importOrderDetailEntity.AddImportOrderDetailAsync(newImportOrderDetail);
                return StatusCode(200, addedImportOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //PUT api/<ImportOrderDetailController>/5
        /// <summary>
        /// Update import order detail
        /// </summary>
        /// <param name="id">ID of the import order detail to be updated</param>
        /// <param name="updatedImportOrderDetail">New information to update the import order detail</param>
        /// <returns>The updated import order detail</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ImportOrderDetail), 200)]
        public async Task<IActionResult> Put(string id, [FromBody] ImportOrderDetail updatedImportOrderDetail)
        {
            try
            {
                if (!id.Equals(updatedImportOrderDetail.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                ImportOrderDetail importOrderDetail = await importOrderDetailEntity.UpdateImportOrderDetailAsync(updatedImportOrderDetail);
                return StatusCode(200, importOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ImportOrderDetailController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await importOrderDetailEntity.DeleteImportOrderDetailAsync(id);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ImportOrderDetailController>/5
        [HttpDelete]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteByImportOrderId([FromQuery] string importOrderId)
        {
            try
            {
                await importOrderDetailEntity.DeleteImportOrderDetailsByImportOrderIdAsync(importOrderId);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
