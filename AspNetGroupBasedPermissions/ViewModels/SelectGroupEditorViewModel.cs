using System.ComponentModel.DataAnnotations;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectGroupEditorViewModel
    {
        // Used to display a single role group with a checkbox, within a list structure:
        public SelectGroupEditorViewModel() { }
        public SelectGroupEditorViewModel(Group group)
        {
            this.GroupName = group.Name;
            this.GroupId = group.Id;
        }
        [Required]
        public int GroupId { get; set; }
        public bool Selected { get; set; }
        public string GroupName { get; set; }
    }
}