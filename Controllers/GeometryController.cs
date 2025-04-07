using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Microsoft.EntityFrameworkCore;
using GeoMap.Models;
using GeoMap.Data;

namespace GeoMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeometryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeometryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddGeometry([FromBody] GeometryDto dto)
        {
            var reader = new GeoJsonReader();
            var geometry = reader.Read<Geometry>(dto.GeoJson);

            string layerName;
            if (string.IsNullOrWhiteSpace(dto.LayerName))
            {
                layerName = "Layer";
                int layerCount = _context.Geometries.Count(g => g.LayerName.StartsWith(layerName));
                if (layerCount > 0)
                {
                    layerName = $"{layerName}_{layerCount + 1}";
                }
            }
            else
            {
                layerName = dto.LayerName.Trim();
            }

            string name = string.IsNullOrWhiteSpace(dto.Name) ? "Drawing" : dto.Name.Trim();
            int nameCount = _context.Geometries.Count(g => g.Name.StartsWith(name));
            if (_context.Geometries.Any(g => g.Name == name))
            {
                name = $"{name}_{nameCount + 1}";
            }

            var newGeometry = new GeometryEntity
            {
                Name = name,
                Description = dto.Description,
                LayerName = layerName,
                Geometry = geometry
            };

            _context.Geometries.Add(newGeometry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Geometry saved", name, layerName });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var geometry = await _context.Geometries.FindAsync(id);

            if (geometry == null)
                return NotFound();

            return Ok(new
            {
                geometry.Id,
                geometry.Name,
                geometry.Description,
                geometry.LayerName,
                Geometry = geometry.Geometry == null ? null : new GeoJsonWriter().Write(geometry.Geometry)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGeometry(int id, [FromBody] UpdateGeometryDto dto)
        {
            var geometry = await _context.Geometries.FindAsync(id);

            if (geometry == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                geometry.Name = dto.Name;

            geometry.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.LayerName))
                geometry.LayerName = dto.LayerName;

            if (!string.IsNullOrWhiteSpace(dto.GeoJson))
            {
                var reader = new GeoJsonReader();
                geometry.Geometry = reader.Read<Geometry>(dto.GeoJson);
            }

            _context.Geometries.Update(geometry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Geometry updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeometry(int id)
        {
            var geometry = await _context.Geometries.FindAsync(id);

            if (geometry == null)
                return NotFound();

            _context.Geometries.Remove(geometry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Geometry deleted successfully" });
        }

        [HttpGet("layer/{layerName}")]
        public IActionResult GetByLayer(string layerName)
        {
            var geometries = _context.Geometries
                .Where(g => g.LayerName == layerName)
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.Description,
                    g.LayerName,
                    Geometry = g.Geometry == null ? null : new GeoJsonWriter().Write(g.Geometry)
                })
                .ToList();

            return Ok(geometries);
        }

        [HttpGet("layers")]
        public IActionResult GetDistinctLayers()
        {
            var layers = _context.Geometries
                .Where(g => !string.IsNullOrEmpty(g.LayerName))
                .Select(g => g.LayerName)
                .Distinct()
                .ToList();

            return Ok(layers);
        }

        [HttpPost("intersect")]
        public IActionResult GetIntersectingGeometries([FromBody] string geoJson)
        {
            var reader = new GeoJsonReader();
            var polygon = reader.Read<Geometry>(geoJson);

            var intersecting = _context.Geometries
                .Where(g => g.Geometry != null && g.Geometry.Intersects(polygon))
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.Description,
                    g.LayerName,
                    Geometry = g.Geometry.AsText()
                })
                .ToList();

            return Ok(intersecting);
        }
    }

    public class GeometryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LayerName { get; set; }
        public string GeoJson { get; set; } = string.Empty;
    }

    public class UpdateGeometryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LayerName { get; set; }
        public string? GeoJson { get; set; }
    }
}