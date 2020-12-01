using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dental_App.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string RoleID { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę roli")]
        [Display(Name = "Rola")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
