


using Microsoft.AspNetCore.Identity;

namespace JobSeeking.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name {  get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Company {  get; set; }
        public bool isValid {  get; set; }
        public string? Avatar {  get; set; }
        public string Roles {  get; set; }


    }
}
