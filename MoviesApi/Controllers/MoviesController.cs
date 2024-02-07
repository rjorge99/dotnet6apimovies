using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using MoviesApi.Services;
using NetTopologySuite.Geometries;
using System.Linq.Dynamic.Core;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : CustomBaseController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger _logger;
        private readonly GeometryFactory _geometryFactory;
        private readonly string _container = "movies";

        public MoviesController(DataContext context, IMapper mapper, IFileStorage fileStorage, ILogger<MoviesController> logger, GeometryFactory geometryFactory) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _logger = logger;
            _geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<MoviesIndexDto>> GetMovies()
        {
            var newReleases = await _context.Movies
                .Where(m => m.ReleaseDate > DateTime.Now)
                .OrderBy(m => m.ReleaseDate)
                .ToListAsync();

            var moviesInTheaters = await _context.Movies
                .Where(m => m.IsInTheaters)
                .ToListAsync();

            return Ok(new MoviesIndexDto
            {
                InTheaters = _mapper.Map<List<MovieDto>>(moviesInTheaters),
                NewReleases = _mapper.Map<List<MovieDto>>(newReleases)
            });
        }

        [HttpGet("filterNear")]
        public async Task<ActionResult<NearMovieTheaterDto>> GetNearMoviesTheaters([FromQuery] NearMovieTheaterFilterDto filter)
        {
            var currentLocation = _geometryFactory.CreatePoint(new Coordinate(filter.Longitud, filter.Latitud));
            var movieTheaters = await _context.MovieTheaters
                .OrderBy(m => m.Ubication.Distance(currentLocation))
                .Where(m => m.Ubication.IsWithinDistance(currentLocation, filter.KmDistancie * 1000))
                .Select(x => new NearMovieTheaterDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitud = x.Ubication.Y,
                    Longitud = x.Ubication.X,
                    DistanceInMeters = Math.Round(x.Ubication.Distance(currentLocation))
                })
                .ToListAsync();

            return Ok(movieTheaters);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<MovieDto>> FilterMovies([FromQuery] MovieFilterDto movieFilter)
        {
            var movieQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(movieFilter.Title)) movieQueryable = movieQueryable.Where(m => m.Title.ToLower().Contains(movieFilter.Title.ToLower()));
            if (movieFilter.InTheaters) movieQueryable = movieQueryable.Where(m => m.IsInTheaters);
            if (movieFilter.NewReleases) movieQueryable = movieQueryable.Where(m => m.ReleaseDate > DateTime.Now);
            if (movieFilter.GenreId != 0)
                movieQueryable = movieQueryable.Where(m =>
                    m.MoviesGenres.Select(mg => mg.GenreId).Contains(movieFilter.GenreId));


            if (!string.IsNullOrEmpty(movieFilter.OrderBy))
            {
                var orderBy = movieFilter.OrderBy;
                var direction = movieFilter.Ascending ? "ascending" : "descending";

                try
                {
                    movieQueryable = movieQueryable.OrderBy($"{orderBy} {direction}"); // Using System.Linq.Dynamic.Core

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
            }

            await HttpContext.InsertPaginationParams(movieQueryable, movieFilter.RegistersPerPage);
            var movies = await movieQueryable.Paginate(movieFilter.Pagination).ToListAsync();

            var moviesDto = _mapper.Map<List<MovieDto>>(movies);
            return Ok(moviesDto);

        }


        [HttpGet("{id:int}", Name = "GetMovieById")]
        public async Task<ActionResult<MovieDetailsDto>> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MoviesActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MoviesGenres).ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound("Movie not found");

            movie.MoviesActors = movie.MoviesActors.OrderBy(a => a.Order).ToList();

            var movieDto = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(movieDto);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie([FromForm] MoviePost moviePost)
        {
            var movie = _mapper.Map<Movie>(moviePost);

            if (moviePost.Poster != null)
            {
                using var memoryStream = new MemoryStream();
                await moviePost.Poster.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(moviePost.Poster.FileName);

                movie.Poster = await _fileStorage.SaveFile(content, extension, _container, moviePost.Poster.ContentType);
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDto>(movie);
            return CreatedAtRoute("GetMovieById", new { id = movieDto.Id }, movieDto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutMovie([FromForm] MoviePost moviePost, int id)
        {
            var movieDb = await _context.Movies
                .Include(m => m.MoviesActors)
                .Include(m => m.MoviesGenres)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieDb == null) return NotFound("Movie not found");

            _mapper.Map(moviePost, movieDb);

            if (moviePost.Poster != null)
            {
                using var memoryStream = new MemoryStream();
                await moviePost.Poster.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(moviePost.Poster.FileName);

                movieDb.Poster = await _fileStorage.EditFile(content, extension, _container, movieDb.Poster, moviePost.Poster.ContentType);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            return await Delete<Movie>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchMovie(JsonPatchDocument<MoviePatchDto> patchDocument, int id)
        {
            return await Patch<Movie, MoviePatchDto>(patchDocument, id);
        }


    }
}
