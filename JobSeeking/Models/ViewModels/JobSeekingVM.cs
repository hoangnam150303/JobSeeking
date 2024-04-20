using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSeeking.Models.ViewModels
{
    public class JobSeekingVM
    {
        [ValidateNever]
       public Job Job{  get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
       public IEnumerable<SelectListItem> Categories {  get; set; }
    }
}
