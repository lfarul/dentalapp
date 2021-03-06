﻿using System.Collections.Generic;


namespace DentalApp.ViewModels
{
    public class EditUserViewModel
    {

        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }

}