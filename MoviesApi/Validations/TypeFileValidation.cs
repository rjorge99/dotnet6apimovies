using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class TypeFileValidation : ValidationAttribute
    {
        private string[] _validTypes { get; set; }

        public TypeFileValidation(params string[] validTypes)
        {
            _validTypes = validTypes;
        }

        public TypeFileValidation(TypeFileEnum type)
        {
            _validTypes = type switch
            {
                TypeFileEnum.Image => new[] { "image/jpeg", "image/png", "image/gif" },
                _ => Array.Empty<string>()
            };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var formFile = value as IFormFile;
            if (formFile == null) return ValidationResult.Success;

            if (!_validTypes.Contains(formFile.ContentType))
                return new ValidationResult($"Type of file should be: {string.Join(",", _validTypes)}");


            return ValidationResult.Success;

        }
    }

    public enum TypeFileEnum
    {
        Image
    }
}
