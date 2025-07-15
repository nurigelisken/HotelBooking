using HotelBooking.Infrastructure.Entities;
using HotelBooking.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HotelRoom>()
                .HasOne(hr => hr.Hotel)
                .WithMany(h => h.HotelRooms)
                .HasForeignKey(hr => hr.HotelId);

            modelBuilder.Entity<HotelRoom>()
                .HasOne(hr => hr.Room)
                .WithMany(r => r.HotelRooms)
                .HasForeignKey(hr => hr.RoomId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.HotelRoom)
                .WithMany(hr => hr.Bookings)
                .HasForeignKey(b => b.HotelRoomId);

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Type = RoomType.Single, Name = "Single", Capacity = 1 },
                new Room { Id = 2, Type = RoomType.Double, Name = "Double", Capacity = 2 },
                new Room { Id = 3, Type = RoomType.Deluxe, Name = "Deluxe", Capacity = 4 }
                );


            modelBuilder.Entity<Hotel>().HasData(
                 new Hotel { Id = 1, Name = "Hotel Rengoku" },
                 new Hotel { Id = 2, Name = "Hotel Konohagakure" },
                 new Hotel { Id = 3, Name = "Hotel Tomorrowland" }
                );

            modelBuilder.Entity<HotelRoom>().HasData(
                 new HotelRoom { Id = 1, HotelId = 1, RoomId = 1, Quantity = 2 },
                 new HotelRoom { Id = 2, HotelId = 1, RoomId = 2, Quantity = 2 },
                 new HotelRoom { Id = 3, HotelId = 1, RoomId = 3, Quantity = 2 },

                 new HotelRoom { Id = 4, HotelId = 2, RoomId = 1, Quantity = 3 },
                 new HotelRoom { Id = 5, HotelId = 2, RoomId = 2, Quantity = 3 },

                 new HotelRoom { Id = 6, HotelId = 3, RoomId = 1, Quantity = 3 },
                 new HotelRoom { Id = 7, HotelId = 3, RoomId = 3, Quantity = 3 }
                );
        }
    }
}
