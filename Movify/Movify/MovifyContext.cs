using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Movify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Movify
{
    public class MovifyContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=Movify;uid=nzwice;pwd=khoinguyenit15;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(c => c.role).HasDefaultValue("customer");

            modelBuilder.Entity<Movie>().Property(c => c.status).HasDefaultValue(true);
            modelBuilder.Entity<Genre>().Property(c => c.status).HasDefaultValue(true);
            modelBuilder.Entity<Theater>().Property(c => c.status).HasDefaultValue(true);
            modelBuilder.Entity<Seat>().Property(c => c.status).HasDefaultValue(true);
            modelBuilder.Entity<Ticket>().Property(c => c.status).HasDefaultValue(true);

            modelBuilder.Entity<Ticket>().
                HasOne(e => e.Seat).
                WithMany(e => e.Tickets).
                HasForeignKey(e => e.seatid)
                .IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Seat>().
                HasOne(e => e.theater).
                WithMany(e => e.seats).
                HasForeignKey(e => e.theaterid).
                IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>().Property(c => c.paymentDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Ticket>().Property(c => c.paymentStatus).HasDefaultValue("pending");
        }

        public DbSet<Theater> Theater { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<MovieShow> MovieShows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
