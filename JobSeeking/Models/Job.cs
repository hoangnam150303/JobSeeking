using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeking.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Salary {  get; set; }
        [ValidateNever]
        public string? CompanyName {  get; set; }
        public string? Logo {  get; set; }
        [Required]
        [BindProperty]
        public string[] Category {  get; set; }
        [Required]
        [ValidateNever]
        public string EmployerId {  get; set; }
        [ForeignKey("EmployerId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
        [ValidateNever]
        public int amountOfCV {  get; set; }
    }
}
