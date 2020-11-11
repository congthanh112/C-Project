using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int r { get; set; }

        [Required]
        public int c { get; set; }

        [ForeignKey("Theater")]
        public int theaterid { get; set; }

        public virtual Theater theater { get; set; }

        public bool status { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(r)}={r.ToString()}, {nameof(c)}={c.ToString()}, {nameof(theater)}={theater}, {nameof(status)}={status.ToString()}}}";
        }

        public virtual ICollection<Ticket> Tickets { get; set; }


    }
}
