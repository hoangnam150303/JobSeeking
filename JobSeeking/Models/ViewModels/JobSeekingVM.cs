using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSeeking.Models.ViewModels
{
    public class JobSeekingVM
    {
       public Category Category{  get; set; }
       public IEnumerable<SelectListItem> User {  get; set; }
    }
}
