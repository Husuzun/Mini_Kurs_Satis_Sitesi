using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    /// <summary>
    /// Generic service interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TDto">The DTO type</typeparam>
    public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        Task<Response<TDto>> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<Response<IEnumerable<TDto>>> GetAllAsync();

        /// <summary>
        /// Gets entities based on a predicate
        /// </summary>
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<Response<TDto>> AddAsync(TDto entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<Response<NoDataDto>> UpdateAsync(TDto entity, int id);

        /// <summary>
        /// Removes an entity by its ID
        /// </summary>
        Task<Response<NoDataDto>> RemoveAsync(int id);
    }
}