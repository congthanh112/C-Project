using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("Seat")]
        public int seatid { get; set; }
        public virtual Seat Seat {get; set;}

        [ForeignKey("Customer")]
        public string email { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("MovieShow")]
        public int movieshowid { get; set; }
        public virtual MovieShow MovieShow { get; set; }
        public bool status { get; set; }

        public string? paymentStatus { get; set; }

        public DateTime? paymentDate { get; set; }
    }
}
