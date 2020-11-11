using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Models
{
    public class Movie
    {
        [Key]               //primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }   // properties

        [Required]        // not null
        public string posterURL { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }

        [DataType(DataType.Date)]                       // map datime cua C# qua datetime2 cua SQL
        public DateTime? releaseDate { get; set; }

        public string? actors { get; set; }             // ?: duoc phep null

        public string? duration { get; set; }

        public string? trailerURL { get; set; }

        [ForeignKey("Genre")]                    // khoa ngoai
        public int genreid { get; set; }
        public virtual Genre genre { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(posterURL)}={posterURL}, {nameof(name)}={name}, {nameof(description)}={description}, {nameof(releaseDate)}={releaseDate.ToString()}, {nameof(actors)}={actors}, {nameof(duration)}={duration}, {nameof(trailerURL)}={trailerURL}, {nameof(genre)}={genre}}}";
        }

        public bool status { get; set; }

        public virtual ICollection<MovieShow> MovieShows { get; set; }           // 1 Movie có thể có nhiều movieshow nên xài Collection
    }
}
