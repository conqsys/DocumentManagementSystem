using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ticket.DataAccess;
using System.Security.Principal;
using Ticket.Base;
using System.ComponentModel.DataAnnotations;
using Ticket.API.Common;
using Ticket.DataAccess.Common;
using Dapper;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.DataAccess.TicketService;

namespace Ticket.API.TicketService
{
    [Route("api/[controller]")]
    public class FileIndexesController : SecuredRepositoryController<IFileIndexesRepository>
    {
        public FileIndexesController(IFileIndexesRepository repository) : base(repository)
        {

        }


        [HttpPost]
        public async Task<IActionResult> SaveFileIndexes([FromBody]List<FileIndexes> fileIndexes)
        {
            try
            {
                var savedFileIndexes = await this.Repository.AddNew(fileIndexes);
                return Ok(savedFileIndexes);

            }
            catch (TicketException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFileIndexes([FromBody]FileIndexes fileIndexes)
        {
            try
            {
                var updatedFileIndexes = await this.Repository.Update(fileIndexes);

                return Ok(updatedFileIndexes);
            }
            catch (TicketException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getListOfFileIndexes")]
        public async Task<IEnumerable<IFileIndexes>> GetListOfFileIndexes()
        {
            return await this.Repository.GetListOfFileIndexes();
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await this.Repository.Delete(id);
                return Ok();
            }
            catch (TicketException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}

