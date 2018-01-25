using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetGroupBasedPermissions.Models
{
    public class Group
    {
        public Group()
        {
        }

        [Key]
        [Required]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual ICollection<ApplicationRoleGroup> Roles { get; set; } = new List<ApplicationRoleGroup>();

    }
}