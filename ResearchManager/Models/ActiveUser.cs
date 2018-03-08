namespace ResearchManager.Models
{
    public class ActiveUser
    {
        public int userID { get; set; }
        public string staffPosition { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string matric { get; set; }

        public ActiveUser()
        {

        }
    }
}