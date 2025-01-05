using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    /// <summary>
    /// Generic service implementation for basic CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TDto">The DTO type</typeparam>
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IUserService _userService;

        public ServiceGeneric(IGenericRepository<TEntity> genericRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserService userService = null)
        {
            _genericRepository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userService = userService;
        }

        public virtual async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _genericRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return Response<TDto>.Success(_mapper.Map<TDto>(entity), 200);
        }

        public virtual async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = await _genericRepository.GetAllAsync();
            return Response<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(entities), 200);
        }

        public virtual async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }

            var dto = _mapper.Map<TDto>(entity);

            // Eğer entity Course ise ve dto CourseDto ise, instructor name'i set et
            if (entity is Course course && dto is CourseDto courseDto && _userService != null)
            {
                if (!string.IsNullOrEmpty(course.InstructorId))
                {
                    var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                    if (instructor.Data != null)
                    {
                        courseDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                    }
                }
            }

            return Response<TDto>.Success(dto, 200);
        }

        public virtual async Task<Response<NoDataDto>> RemoveAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            _genericRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public virtual async Task<Response<NoDataDto>> UpdateAsync(TDto dto, int id)
        {
            var existingEntity = await _genericRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            var entity = _mapper.Map<TEntity>(dto);
            _genericRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public virtual async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);
            return Response<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
        }
    }
}