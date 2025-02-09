namespace drawapi.Data.Dtos
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DrawId { get; set; }
        public List<TeamDTO> Teams { get; set; }
    }
}
