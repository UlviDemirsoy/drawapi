using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace drawapi.Data.Models
{
    public class Group
    {
      
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<Team> Teams { get; set; } = new();

        public int DrawId { get; set; }

        [ForeignKey("DrawId")]
        [JsonIgnore]
        public Draw Draw { get; set; }

        public List<GroupTeam> GroupTeams { get; set; } = new();

    }
}
