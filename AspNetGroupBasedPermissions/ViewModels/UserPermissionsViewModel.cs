using System.Collections.Generic;
using AspNetGroupBasedPermissions.Models;

namespace AspNetGroupBasedPermissions.ViewModels
{
    public class UserPermissionsViewModel
    {
        public UserPermissionsViewModel()
        {
            this.Roles = new List<RoleViewModel>();
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}