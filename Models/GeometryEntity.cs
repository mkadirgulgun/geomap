using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace GeoMap.Models
{
    public class GeometryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string LayerName { get; set; } = string.Empty;

        public Geometry? Geometry { get; set; }
    }
}
