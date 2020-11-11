using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Models
{
    public class Theater
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public bool status { get; set; }

        [Required]
        public int rows { get; set; }

        [Required]
        public int cols { get; set; }

        public ICollection<Seat> seats { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(name)}={name}, {nameof(status)}={status.ToString()}, {nameof(seats)}={seats}}}";
        }
    }
}
