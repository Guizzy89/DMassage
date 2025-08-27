using DMassage.Models;
using Microsoft.EntityFrameworkCore;

namespace DMassage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Massage> Massages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}