using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dental_App.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Proszę podać rolę")]
        [Display(Name = "Rola")]
        public string RoleName { get; set; }
    }
}
