using System.ComponentModel.DataAnnotations;


namespace DentalApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please provide first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Pesel
        [Required(ErrorMessage = "Please provide 11 digit identity number")]
        [RegularExpression(@"^[0-9]{11}$")]
        public string Pesel { get; set; }

        //Email
        [Required(ErrorMessage = "Please provide email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Hasło
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Potwierdzenie hasła
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Provided passwords are not the same")]
        public string ConfirmPassword { get; set; }
    }
}
