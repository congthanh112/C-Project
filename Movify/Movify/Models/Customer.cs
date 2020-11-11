using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Models
{
    public class Customer
    {
        [Key]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [AllowNull]
        public string? address { get; set; }

        public string role { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(email)}={email}, {nameof(password)}={password}, {nameof(phone)}={phone}, {nameof(dob)}={dob.ToString()}, {nameof(address)}={address}}}";
        }

        public bool status { get; set; }
    }
}
