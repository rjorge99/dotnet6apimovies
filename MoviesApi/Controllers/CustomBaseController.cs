using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;
using MoviesApi.Helpers;

namespace MoviesApi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomBaseController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entities = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<List<TDTO>>(entities);
            return dtos;
        }

        protected async Task<ActionResult<List<TDTO>>> Get<TEntity, TDTO>(PaginationDto pagination) where TEntity : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            return await Get<TEntity, TDTO>(pagination, queryable);
        }

        protected async Task<ActionResult<List<TDTO>>> Get<TEntity, TDTO>(PaginationDto pagination, IQueryable<TEntity> queryable) where TEntity : class
        {
            await HttpContext.InsertPaginationParams(queryable, pagination.RegistersPerPage);
            var entities = await queryable.Paginate(pagination).ToListAsync();
            var entitiesDto = _mapper.Map<List<TDTO>>(entities);

            return Ok(entitiesDto);
        }

        protected async Task<ActionResult<TDTO>> GetById<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return NotFound();

            var entityDto = _mapper.Map<TDTO>(entity);
            return Ok(entityDto);
        }

        protected async Task<ActionResult> Post<TEntity, TPost, TLecture>(TPost postDto, string pathName) where TEntity : class, IId
        {
            var entity = _mapper.Map<TEntity>(postDto);

            _context.Add(entity);
            await _context.SaveChangesAsync();


            var lectureEntity = _mapper.Map<TLecture>(entity);
            return CreatedAtRoute(pathName, new { id = entity.Id }, lectureEntity);
        }

        protected async Task<ActionResult> Put<TEntity, TPost>(TPost postDto, int id) where TEntity : class, IId
        {
            var entityDb = await _context.Set<TEntity>().AnyAsync(g => g.Id == id);
            if (!entityDb) return NotFound();

            var entity = _mapper.Map<TEntity>(postDto);
            entity.Id = id;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IId, new()
        {
            var entityExists = await _context.Set<TEntity>().AnyAsync(g => g.Id == id);
            if (!entityExists) return NotFound();

            _context.Remove(new TEntity { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntity, TPatchDto>(JsonPatchDocument<TPatchDto> patchDocument, int id)
            where TEntity : class, IId
            where TPatchDto : class
        {
            if (patchDocument == null) return BadRequest();

            var entityDb = await _context.Set<TEntity>().FirstOrDefaultAsync(a => a.Id == id);
            if (entityDb == null) return NotFound();

            var entityPatchDto = _mapper.Map<TPatchDto>(entityDb);
            patchDocument.ApplyTo(entityPatchDto, ModelState);

            var isValid = TryValidateModel(entityPatchDto);
            if (!isValid) return BadRequest(ModelState);

            _mapper.Map(entityPatchDto, entityDb);
            await _context.SaveChangesAsync();


            return NoContent();
        }
    }
}
