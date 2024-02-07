using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class ActorPost
    {
        [Required]
        [StringLength(maximumLength: 120)]
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        [SizePhotoValidation(megaBytesMaxSize: 4)]
        [TypeFileValidation(TypeFileEnum.Image)]
        public IFormFile Photo { get; set; }
    }

}
