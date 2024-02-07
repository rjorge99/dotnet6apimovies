﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Genre : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string Name { get; set; }
        public ICollection<MoviesGenres> MoviesGenres { get; set; }
    }
}
