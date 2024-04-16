using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSeeking.Models.ViewModels
{
    public class JobSeekingVM
    {
       public Job Job{  get; set; }
        public Category category { get; set; }
        [ValidateNever]
       public IEnumerable<SelectListItem> Category {  get; set; }
       /* public List<string> SelectedCategories { get; set; }*/
    }
}
