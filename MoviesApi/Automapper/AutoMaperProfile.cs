using AutoMapper;
using MoviesApi.Dtos;
using MoviesApi.Entities;
using NetTopologySuite.Geometries;

namespace MoviesApi.Automapper
{
    public class AutoMaperProfile : Profile
    {
        public AutoMaperProfile(GeometryFactory geometryFactory)
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenrePost, Genre>();

            CreateMap<Review, ReviewDto>()
                .ForMember(r => r.UserName, options => options.MapFrom(x => x.User.UserName));
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewPostDto, Review>();

            CreateMap<MovieTheater, MovieTheaterDto>()
                .ForMember(m => m.Latitud, options => options.MapFrom(ma => ma.Ubication.Y))
                .ForMember(m => m.Longitud, options => options.MapFrom(ma => ma.Ubication.X));



            CreateMap<MovieTheaterDto, MovieTheater>()
                .ForMember(m => m.Ubication,
                    o => o.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<MovieTheaterPostDto, MovieTheater>()
                .ForMember(m => m.Ubication,
                    o => o.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<ActorDto, Actor>().ReverseMap();
            CreateMap<ActorPost, Actor>().ForMember(a => a.Photo, options => options.Ignore());

            CreateMap<ActorPatchDto, Actor>().ReverseMap();


            CreateMap<MovieDto, Movie>().ReverseMap();
            CreateMap<MoviePost, Movie>()
                .ForMember(a => a.Poster, options => options.Ignore())
                .ForMember(m => m.MoviesGenres, options => options.MapFrom(MapMovieGenres))
                .ForMember(m => m.MoviesActors, options => options.MapFrom(MapMovieActor));

            CreateMap<Movie, Movie>().ReverseMap();

            CreateMap<Movie, MovieDetailsDto>()
                .ForMember(m => m.Genres, options => options.MapFrom(MapMovieGenres))
                .ForMember(m => m.Actors, options => options.MapFrom(MapMovieActors));



            CreateMap<MoviePatchDto, Movie>().ReverseMap();
        }


        private List<ActorMovieDetailDto> MapMovieActors(Movie movie, MovieDetailsDto movieDetailDto)
        {
            var actors = new List<ActorMovieDetailDto>();
            if (movie.MoviesActors == null) return actors;

            foreach (var movieActors in movie.MoviesActors)
                actors.Add(new ActorMovieDetailDto() { ActorId = movieActors.ActorId, Character = movieActors.Character, PersonName = movieActors.Actor.Name });

            return actors;
        }


        private List<GenreDto> MapMovieGenres(Movie movie, MovieDetailsDto movieDetailDto)
        {
            var genresDto = new List<GenreDto>();
            if (movie.MoviesGenres == null) return genresDto;


            foreach (var genre in movie.MoviesGenres)
                genresDto.Add(new GenreDto() { Id = genre.GenreId, Name = genre.Genre.Name });

            return genresDto;
        }


        private ICollection<MoviesGenres> MapMovieGenres(MoviePost moviePost, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (moviePost.GenresIds == null) return result;


            foreach (var genreId in moviePost.GenresIds)
                result.Add(new MoviesGenres() { GenreId = genreId });

            return result;
        }


        private ICollection<MoviesActors> MapMovieActor(MoviePost moviePost, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (moviePost == null) return result;


            var count = 1;
            foreach (var actor in moviePost.Actors)
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character, Order = count++ });


            return result;
        }
    }
}
