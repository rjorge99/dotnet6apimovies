using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : CustomBaseController
    {
        public GenresController(DataContext _context, IMapper _mapper)
            : base(_context, _mapper)
        {
        }


        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetGenres()
        {
            return await Get<Genre, GenreDto>();
        }

        [HttpGet("{id:int}", Name = "GetGenreById")]
        public async Task<ActionResult<GenreDto>> GetGenreById(int id)
        {
            return await GetById<Genre, GenreDto>(id);
        }


        [HttpPost]
        public async Task<ActionResult<GenreDto>> PostGenre(GenrePost genrePost)
        {
            return await Post<Genre, GenrePost, GenreDto>(genrePost, "GetGenreById");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutGenre(GenrePost genrePost, int id)
        {
            return await Put<Genre, GenrePost>(genrePost, id);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            return await Delete<Genre>(id);
        }

    }
}
