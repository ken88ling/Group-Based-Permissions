using System.ComponentModel.DataAnnotations;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectRoleEditorViewModel
    {
        // Used to display a single role with a checkbox, within a list structure:
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(ApplicationRole role)// Update this to accept an argument of type ApplicationRole:
        {
            this.RoleName = role.Name;
            this.Description = role.Description;   // Assign the new Descrption property:
        }
        
        [Required]
        public string RoleName { get; set; }
        public bool Selected { get; set; }
        public string Description { get; set; }// Add the new Description property:
    }
}