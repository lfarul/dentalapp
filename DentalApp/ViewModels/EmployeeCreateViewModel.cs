using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace DentalApp.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Please provide first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please provide first name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide specialty")]
        public string Specialty { get; set; }


        [Required(ErrorMessage = "Please provide email")]
        [RegularExpression(@"^[a-zA-Z0-9_+.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "This is not an appropriate format")]
        public string Email { get; set; }

        public IFormFile Photo { get; set; }


        [Required(ErrorMessage = "Please provide description")]
        public string Description { get; set; }
    }
}
