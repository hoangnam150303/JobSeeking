using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeking.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isValid {  get; set; }
        public DateTime CreateDay { get; set; } = DateTime.Now;

        
    }
}
