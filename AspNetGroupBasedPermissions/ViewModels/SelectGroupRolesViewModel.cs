using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web.UI.WebControls.Expressions;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class SelectGroupRolesViewModel
    {
        public SelectGroupRolesViewModel()
        {
            Roles = new List<SelectRoleEditorViewModel>();
        }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    public class SelectRoleEditorViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public bool Selected { get; set; }
        public string Description { get; set; }
    }
}