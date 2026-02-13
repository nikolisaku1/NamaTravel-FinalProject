using Microsoft.EntityFrameworkCore;
using NamaTravelApi.Models;

namespace NamaTravelApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Booking> Bookings => Set<Booking>();
}
