namespace Dental_App.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int EmployeeID { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
