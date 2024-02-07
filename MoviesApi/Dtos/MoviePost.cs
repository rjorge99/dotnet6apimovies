using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using MoviesApi.Validations;

namespace MoviesApi.Dtos
{
    public class MoviePost : MoviePatchDto
    {
        [SizePhotoValidation(megaBytesMaxSize: 4)]
        [TypeFileValidation(TypeFileEnum.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(typeof(TypeBinder<ICollection<int>>))]
        public ICollection<int> GenresIds { get; set; }

        [ModelBinder(typeof(TypeBinder<ICollection<ActorMoviePostDto>>))]
        public ICollection<ActorMoviePostDto> Actors { get; set; }
    }
}
