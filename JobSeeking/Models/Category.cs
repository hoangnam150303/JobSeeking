namespace JobSeeking.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDay { get; set; } = DateTime.Now;
        
    }
}
