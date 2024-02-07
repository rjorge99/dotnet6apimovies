namespace MoviesApi.Dtos;

public class NearMovieTheaterDto : MovieTheaterDto
{
    public double DistanceInMeters { get; set; }
}