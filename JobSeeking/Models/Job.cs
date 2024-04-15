using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeking.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Salary {  get; set; }
        public string CompanyName {  get; set; }
        public string? Logo {  get; set; }
        [BindProperty]
        public string[] Category {  get; set; }
        [ValidateNever]
        public string EmployerId {  get; set; }
        [ForeignKey("EmployerId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
