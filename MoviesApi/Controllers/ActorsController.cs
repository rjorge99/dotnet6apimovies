using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : CustomBaseController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly string _container = "actors";

        public ActorsController(DataContext context, IMapper mapper,
            IFileStorage _fileStorage
            ) : base(context, mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._fileStorage = _fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> GetActors([FromQuery] PaginationDto pagination)
        {
            return await Get<Actor, ActorDto>(pagination);
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDto>> GetActorById(int id)
        {
            return await GetById<Actor, ActorDto>(id);
        }

        [HttpPost]
        public async Task<ActionResult<ActorDto>> PostActor([FromForm] ActorPost actorPost)
        {
            var actor = _mapper.Map<Actor>(actorPost);

            if (actorPost.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                await actorPost.Photo.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(actorPost.Photo.FileName);

                actor.Photo = await _fileStorage.SaveFile(content, extension, _container, actorPost.Photo.ContentType);
            }

            _context.Add(actor);
            await _context.SaveChangesAsync();

            var actorDto = _mapper.Map<ActorDto>(actor);
            return CreatedAtRoute("GetActorById", new { id = actorDto.Id }, actor);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ActorDto>> PutActor([FromForm] ActorPost actorPost, int id)
        {
            var actorDb = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            if (actorDb == null) return NotFound("Actor not found");

            _mapper.Map(actorPost, actorDb);

            if (actorPost.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                await actorPost.Photo.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(actorPost.Photo.FileName);

                actorDb.Photo = await _fileStorage.EditFile(content, extension, _container, actorDb.Photo, actorPost.Photo.ContentType);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {

            return await Delete<Actor>(id);
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchActor(JsonPatchDocument<ActorPatchDto> patchDocument, int id)
        {
            return await Patch<Actor, ActorPatchDto>(patchDocument, id);
        }
    }
}
