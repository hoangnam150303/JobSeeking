
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace JobSeeking.Models
{
    public class ApplicationUser:IdentityUser
    {
        
        public string Name {  get; set; }
        [ValidateNever]
        public string Address { get; set; }
        [ValidateNever]
        public string City { get; set; }
        [ValidateNever]
        public string Company {  get; set; }
        [ValidateNever]
        public bool isValid {  get; set; }
        [ValidateNever]
        public string? Avatar {  get; set; }
        


    }
}
