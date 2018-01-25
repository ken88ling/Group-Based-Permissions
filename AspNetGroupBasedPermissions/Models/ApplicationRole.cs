using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AspNetGroupBasedPermissions.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual string Description { get; set; }
    }
}