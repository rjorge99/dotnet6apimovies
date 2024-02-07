using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class SizePhotoValidation : ValidationAttribute
    {
        private readonly int _megaBytesMaxSize;

        public SizePhotoValidation(int megaBytesMaxSize)
        {
            _megaBytesMaxSize = megaBytesMaxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;


            var formFile = value as FormFile;
            if (formFile == null) return ValidationResult.Success;

            if (formFile.Length > _megaBytesMaxSize * 1024 * 1024)
                return new ValidationResult($"Size should not be higher than {_megaBytesMaxSize}mb");

            return ValidationResult.Success;
        }
    }
}
