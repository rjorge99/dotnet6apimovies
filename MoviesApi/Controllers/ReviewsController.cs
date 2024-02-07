using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.Dtos;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/reviews")]
    [ServiceFilter(typeof(MovieExistsAttribute))]
    public class ReviewsController : CustomBaseController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> Get(int movieId,
            [FromQuery] PaginationDto paginacionDTO)
        {
            var queryable = _context.Reviews.Include(x => x.User).AsQueryable();
            queryable = queryable.Where(x => x.MovieId == movieId);
            return await Get<Review, ReviewDto>(paginacionDTO, queryable);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int movieId, [FromBody] ReviewPostDto reviewPostDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var reviewExists = await _context.Reviews
                .AnyAsync(x => x.MovieId == movieId && x.UserId == userId);

            if (reviewExists) return BadRequest("The user already wrote a review");


            var review = _mapper.Map<Review>(reviewPostDto);
            review.MovieId = movieId;
            review.UserId = userId;

            _context.Add(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int movieId, int reviewId,
            [FromBody] ReviewPostDto reviewPostDto)
        {
            var reviewDB = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            if (reviewDB == null) return NotFound("Review not found");

            var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (reviewDB.UserId != usuarioId)
                return Forbid();

            _mapper.Map(reviewPostDto, reviewDB);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int reviewId)
        {
            var reviewDB = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            if (reviewDB == null) return NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (reviewDB.UserId != userId) return Forbid();

            _context.Remove(reviewDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
