using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeking.Models
{
    public class ApplyCV
    {
        public int Id {  get; set; }
        [ValidateNever]
        public int JobId {  get; set; }
        [ForeignKey("JobId")]
        [ValidateNever]
        public Job Job { get; set; }
        [ValidateNever]
        public string JobSeekerEmail {  get; set; }
        public DateTime TimeCreate { get; set; }= DateTime.Now;
        public string Description {  get; set; }
        [ValidateNever]
        public string CV {  get; set; }
    }
}
