using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeking.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool isValid {  get; set; }
        [ValidateNever]
        public string EmployerId {  get; set; }
        [ForeignKey("EmployerId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
        public DateTime CreateDay { get; set; } = DateTime.Now;

        public bool categoryValid { get; set; }
    }
}
