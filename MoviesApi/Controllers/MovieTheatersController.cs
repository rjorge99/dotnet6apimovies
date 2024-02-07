using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movietheaters")]
    public class MovieTheatersController : CustomBaseController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MovieTheatersController(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDto>>> Get()
        {
            return await Get<MovieTheater, MovieTheaterDto>();
        }

        [HttpGet("{id:int}", Name = "GetMovieTheaterById")]
        public async Task<ActionResult<MovieTheaterDto>> Get(int id)
        {
            return await GetById<MovieTheater, MovieTheaterDto>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MovieTheaterPostDto movieDto)
        {
            return await Post<MovieTheater, MovieTheaterPostDto, MovieTheaterDto>(movieDto, "GetMovieTheaterById");
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] MovieTheaterPostDto movieDto, int id)
        {
            return await Put<MovieTheater, MovieTheaterPostDto>(movieDto, id);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<MovieTheater>(id);
        }
    }
}
