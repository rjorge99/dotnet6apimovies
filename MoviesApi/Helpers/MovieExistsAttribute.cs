﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;

namespace MoviesApi.Helpers
{
    public class MovieExistsAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly DataContext _dbContext;

        public MovieExistsAttribute(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var movieIdObject = context.HttpContext.Request.RouteValues["movieId"];

            if (movieIdObject == null) return;

            var movieId = int.Parse(movieIdObject.ToString());

            var movieExists = await _dbContext.Movies.AnyAsync(x => x.Id == movieId);

            if (!movieExists)
                context.Result = new NotFoundResult();
            else
                await next();
        }
    }
}
