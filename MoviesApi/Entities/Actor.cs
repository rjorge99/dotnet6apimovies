using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Actor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 120)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Photo { get; set; }
        public ICollection<MoviesActors> MoviesActors { get; set; }

    }
}
