using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movify.Models
{
    public class MovieShow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public DateTime startTime { get; set; }

        [Required]
        public DateTime endTime { get; set; }

        [Required]
        public double price { get; set; }

        [ForeignKey("Theater")]
        public int theaterid { get; set; }

        [ForeignKey("Movie")]
        public int movieid { get; set; }
        public virtual Theater theater { get; set; }
        public virtual Movie movie { get; set; }

        public bool status { get; set; }

        public ICollection<Ticket> tickets { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(startTime)}={startTime.ToString()}, {nameof(endTime)}={endTime.ToString()}, {nameof(price)}={price.ToString()}, {nameof(theaterid)}={theaterid.ToString()}, {nameof(movieid)}={movieid.ToString()}, {nameof(theater)}={theater}, {nameof(movie)}={movie}, {nameof(status)}={status.ToString()}, {nameof(tickets)}={tickets}}}";
        }
    }
}