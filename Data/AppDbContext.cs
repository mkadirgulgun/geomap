using GeoMap.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoMap.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<GeometryEntity> Geometries { get; set; }
    }
}
