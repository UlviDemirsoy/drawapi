using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace drawapi.Data.Models
{
    public class Team
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required] 
        [MaxLength(200)]
        public string Country { get; set; } = string.Empty;

        public List<GroupTeam> GroupTeams { get; set; } = new();
    }
}
