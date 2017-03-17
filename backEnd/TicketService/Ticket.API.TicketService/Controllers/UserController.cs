using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ticket.API.Common;
using Ticket.DataAccess.TicketService;
using Ticket.DataAccess.Common;
using Ticket.Base.Entities;
using Ticket.Base.Repositories;
using Ticket.Base.Services;
using Ticket.Base.Objects;


namespace Ticket.API.TicketService
{
    [Route("api/[controller]")]
    public class UserController : SecuredRepositoryController<IUserRepository>
    {
        private IUserService _userService;
        public UserController(IUserRepository repository
           )
            : base(repository)
        {
        }

        [HttpGet("me")]
        public async Task<IUser> GetIdenity()
        {
            /* this is TicketIdentity now*/
            return this.Identity.User;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]User newUser)
        {
            try
            {
                var savedUser = await this.Repository.AddNew(newUser);
                return Ok(savedUser);
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
        public async Task<IActionResult> UpdateUser([FromBody]User updateUser)
        {
            try
            {
                var updatedUser = await this.Repository.Update(updateUser);
                return Ok(updatedUser);
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

        [HttpGet("{enabledRequired}/list")]
        public async Task<IEnumerable<IUser>> GetAllUsers()
        {
            return await this.Repository.FindAllUsersList();
        }
        
        [HttpGet("{roleId}/rolewiseuserlist")]
        public async Task<IEnumerable<IUser>> GetUsersByRoleId(long roleId)
        {
            return await this.Repository.FindAllByRoleId(roleId);
        }

        [HttpPost("{id}/disable")]
        public async Task<IActionResult> DisableUser(long id)
        {
            var updatedUser = await this.Repository.ChangeActiveState(id, false, this.Identity.User.Id);
            return Ok(updatedUser);
        }

        [HttpPost("{id}/enable")]
        public async Task<IActionResult> EnableUser(long id)
        {
            try
            {
                var updatedUser = await this.Repository.ChangeActiveState(id, true, this.Identity.User.Id);
                return Ok(updatedUser);
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

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await this._userService.LogOutUser(this.Identity.User.Id);
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

        [HttpPost("{id}/changepassword/{password}")]
        public async Task<IActionResult> ChangePassword(long id, string password)
        {
            try
            {
                await this._userService.ChangePassword(id, password);
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

        [HttpGet("getUserById/{id}")]
        public async Task<object> FindUserById(long id)
        {
            return await this.Repository.FindById(id);
        }

        [HttpGet("getAgentUsersByUserCoordinatorId/{coordinatorId}")]
        public async Task<object> GetAgentsUsersByUserCoordinatorId(long coordinatorId)
        {
            return await this.Repository.GetAgentsUsersByUserCoordinatorId(coordinatorId);
        }

        [HttpGet("getClientsUserByClientId/{accountId}")]
        public async Task<object> GetClientsUserByClientId(long accountId)
        {
            return await this.Repository.GetClientsUserByClientId(accountId);
        }


        [HttpPost("resetpassword/{oldPassword}/{newPassword}")]
        public async Task<IActionResult> ResetPassowrd(string oldPassword, string newPassword)
        {
            try
            {
                await this.Repository.ResetPassowrd(Identity.User.Id, oldPassword, newPassword);
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
