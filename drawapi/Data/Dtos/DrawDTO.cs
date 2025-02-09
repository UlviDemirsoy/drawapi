namespace drawapi.Data.Dtos
{
    public class DrawDTO
    {
        public int Id { get; set; }
        public string DrawerName { get; set; }
        public DateTime DrawDate { get; set; }
        public List<GroupDTO> Groups { get; set; }
    }
}
