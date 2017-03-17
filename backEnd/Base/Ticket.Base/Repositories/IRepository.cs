using Ticket.Base.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Base.Repositories
{
    public interface IRepository<T>  where T : class, IEntity
    {

        Task<bool> Exists(string name);

        Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(T entity);

        Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entityNew, T entityDb);

        Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entity);

        Task<IEnumerable<IValidationResult>> ValidateEntity(T entityNew);

    }
}
