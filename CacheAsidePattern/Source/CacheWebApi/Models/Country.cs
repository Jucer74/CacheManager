using System.ComponentModel.DataAnnotations;

namespace CacheWebApi.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Iso2 { get; set; }

        [Required]
        public string Iso3 { get; set; }
    }
}