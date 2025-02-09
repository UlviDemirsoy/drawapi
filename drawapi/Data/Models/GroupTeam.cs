namespace drawapi.Data.Models
{
    public class GroupTeam
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
