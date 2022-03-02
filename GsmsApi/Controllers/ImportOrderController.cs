using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GsmsApi.Controllers
{
    //PhucVVT
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/import-orders")]
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
        /// <param name="startDate" name="endDate">Filter by startDate and endDate</param>
        /// <param name="searchByName">Search by Import Order Name</param>
        /// <param name="sortByName">Sort by Import Order Name</param>
        /// <param name="sortByDate">Sort by Import Order Created Date</param>
        /// <param name="page">Page number, 0 to get all</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>List of import orders</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<ImportOrder>), 200)]
        public async Task<IActionResult> GetAsync(
            [FromQuery(Name = "start-date")] DateTime? startDate, 
            [FromQuery(Name = "end-date")] DateTime? endDate,
            [FromQuery(Name = "search-by-name")] string searchByName,
            [FromQuery(Name = "sort-by-name")] SortType? sortByName,
            [FromQuery(Name = "sort-by-date")] SortType? sortByDate,
            [FromQuery] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10)
        {
            try
            {
                IEnumerable<ImportOrder> importOrders = await importOrderEntity
                    .GetImportOrdersAsync(startDate, endDate, searchByName, sortByName, sortByDate, page, pageSize);
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
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                ImportOrder importOrder = await importOrderEntity.GetAsync(id);
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
        [ProducesResponseType(typeof(ImportOrder), 200)]
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
                return StatusCode(200, addedImportOrder);
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
        [ProducesResponseType(typeof(ImportOrder), 200)]
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
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await importOrderEntity.DeleteImportOrderAsync(id);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
