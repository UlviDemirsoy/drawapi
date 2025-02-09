using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace drawapi.Data.Models
{
    public class Draw
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DrawerName { get; set; } = string.Empty;

        [Required]
        public DateTime DrawDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public List<Group> Groups { get; set; } = new();
    }
}
