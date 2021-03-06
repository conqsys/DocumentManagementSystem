﻿using System;
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
    public class ImportFileController : SecuredRepositoryController<IImportFileRepository>
    {
        public ImportFileController(IImportFileRepository repository) : base(repository)
        {

        }


        [HttpPost]
        public async Task<IActionResult> SaveImportFile([FromBody]ImportFile importFile)
        {
            try
            {
                var savedImportFile = await this.Repository.AddNew(importFile);
                return Ok(savedImportFile);
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
        public async Task<IActionResult> UpdateImportFile([FromBody]ImportFile importFile)
        {
            try
            {
                var updatedImportFile = await this.Repository.Update(importFile);

                return Ok(updatedImportFile);
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

