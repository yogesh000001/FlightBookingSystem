using FlightBookingSystem.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repository.Context
{
    public class FlightContext : DbContext
    {
        public FlightContext() { }

        public FlightContext(DbContextOptions<FlightContext> options)
            : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PassengerEntity> Passengers { get; set; }
        public DbSet<FlightEntity> Flights { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<CancellationEntity> Cancellations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<PassengerEntity>()
                .HasOne(p => p.User)
                .WithMany(u => u.Passengers)
                .HasForeignKey(p => p.BookedByUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FlightEntity>().ToTable("Flight").HasKey(f => f.FlightID);

            modelBuilder
                .Entity<FlightEntity>()
                .Property(f => f.FlightNumber)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<FlightEntity>().Property(f => f.DepartureTime).IsRequired();

            modelBuilder.Entity<FlightEntity>().Property(f => f.ArrivalTime).IsRequired();

            modelBuilder
                .Entity<FlightEntity>()
                .Property(f => f.Origin)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Entity<FlightEntity>()
                .Property(f => f.Destination)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Entity<FlightEntity>()
                .Property(f => f.Status)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<FlightEntity>().Property(f => f.TotalSeats).IsRequired();

            modelBuilder
                .Entity<BookingEntity>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<BookingEntity>()
                .HasOne(b => b.Passenger)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PassengerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingEntity>().Property(u => u.SeatNumber).IsRequired(false);

            modelBuilder
                .Entity<CancellationEntity>()
                .HasOne(c => c.Booking)
                .WithMany(b => b.Cancellations)
                .HasForeignKey(c => c.BookingID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}