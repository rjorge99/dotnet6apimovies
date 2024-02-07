using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class NearMovieTheaterFilterDto
    {

        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }

        private int _kmsDistance = 10;
        private int _maxKmsDistance = 50;

        public int KmDistancie
        {
            get => _kmsDistance;
            set => _kmsDistance = value > _maxKmsDistance ? _maxKmsDistance : value;
        }
    }
}
