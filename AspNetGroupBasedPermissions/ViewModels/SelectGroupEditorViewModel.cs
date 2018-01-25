using System.ComponentModel.DataAnnotations;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectGroupEditorViewModel
    {
        //public SelectGroupEditorViewModel() { }
        //public SelectGroupEditorViewModel(Group group)
        //{
        //    this.GroupName = group.Name;
        //    this.GroupId = group.Id;
        //}
        [Required]
        public int GroupId { get; set; }
        public bool Selected { get; set; }
        public string GroupName { get; set; }
    }
}