using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsmsApi.Controllers
{
    //PhucVVT
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptDetailController : ControllerBase
    {
        private ReceiptDetailBusinessEntity receiptDetailEntity;
        public ReceiptDetailController(IUnitOfWork work)
        {
            receiptDetailEntity = new ReceiptDetailBusinessEntity(work);
        }

        // GET: api/<ReceiptDetailController>
        /// <summary>
        /// Get All the receipt details
        /// </summary>
        /// <returns>List of receipt details</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<ReceiptDetail>), 200)]
        public async Task<IActionResult> GetsAsync([FromQuery] string receiptId) 
        {
            try
            {
                IEnumerable<ReceiptDetail> receiptDetails;
                if (string.IsNullOrEmpty(receiptId))
                {
                    receiptDetails = await receiptDetailEntity.GetReceiptDetailsAsync();
                }
                else
                {
                    receiptDetails = await receiptDetailEntity.GetReceiptDetailsByReceiptIdAsync(receiptId);
                }
                return StatusCode(200, receiptDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ReceiptDetailController>/5
        /// <summary>
        /// Get receipt detail based on ID
        /// </summary>
        /// <param name="id">The ID string of the receipt detail</param>
        /// <returns>The existed receipt detail</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ReceiptDetail), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                ReceiptDetail receiptDetail = await receiptDetailEntity.GetAsync(id);
                if (receiptDetail == null)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, receiptDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // POST api/<ReceiptDetailController>
        /// <summary>
        /// Add a new receipt detail
        /// </summary>
        /// <param name="newReceiptDetail">New receipt detail to be added</param>
        /// <returns>The added receipt detail</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReceiptDetail), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody] ReceiptDetail newReceiptDetail)
        {
            try
            {
                ReceiptDetail addedReceiptDetail = await receiptDetailEntity.AddReceiptDetailAsync(newReceiptDetail);
                return StatusCode(201, addedReceiptDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //PUT api/<ReceiptDetailController>/5
        /// <summary>
        /// Update receipt detail
        /// </summary>
        /// <param name="id">ID of the receipt detail to be updated</param>
        /// <param name="updatedReceiptDetail">New information to update the receipt detail</param>
        /// <returns>The updated receipt detail</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ReceiptDetail), 201)]
        public async Task<IActionResult> Put(string id, [FromBody] ReceiptDetail updatedReceiptDetail)
        {
            try
            {
                if (!id.Equals(updatedReceiptDetail.Id))
                {
                    throw new Exception("The ID is not the same!!");
                }
                ReceiptDetail receiptDetail = await receiptDetailEntity.UpdateReceiptDetailAsync(updatedReceiptDetail);
                return StatusCode(200, receiptDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ReceiptDetailController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await receiptDetailEntity.DeleteReceiptDetailAsync(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ReceiptDetailController>/5
        [HttpDelete]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteByRecepitId([FromQuery] string receiptId)
        {
            try
            {
                await receiptDetailEntity.DeleteReceiptDetailsByReceiptIdAsync(receiptId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
