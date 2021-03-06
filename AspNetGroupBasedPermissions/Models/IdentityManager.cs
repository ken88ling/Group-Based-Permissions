﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetGroupBasedPermissions.Models
{
    public class IdentityManager
    {
        // Swap ApplicationRole for IdentityRole:
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }
        
        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            return _userManager.Create(user, password);
        }

        // initial use 
        public IdentityResult CreateRole(string name, string description = "")
        {
            var appRole = new ApplicationRole()
            {
                Name = name,
                Description = description
            };
            return _roleManager.Create(appRole);
        }


        public IdentityResult AddUserToRole(string userId, string roleName)
        {
            return _userManager.AddToRole(userId, roleName);
        }

        public void RemoveRolesByUserId(string userId)
        {
            if (_userManager.GetRoles(userId) == null) return;
            foreach (var item in _userManager.GetRoles(userId))
            {
                _userManager.RemoveFromRole(userId, item);
            }
        }

        public void DeleteRole(string roleId)
        {
            var roleUsers = _context.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = _context.Roles.Find(roleId);

            //var roleUsersList = roleUsers.ToList();

            foreach (ApplicationUser user in roleUsers)
            {
                _userManager.RemoveFromRole(user.Id, role.Name);
            }
            _context.Roles.Remove(role);
            _context.SaveChanges();
        }

        public void CreateGroup(string groupName)
        {
            if (IsGroupNameExists(groupName))
            {
                throw new GroupExistsException("A group by that name already exists in the database. Please choose another name.");
            }

            var newGroup = new Group() { Name = groupName };
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
        }

        public bool IsGroupNameExists(string groupName)
        {
            return _context.Groups.Any(gr => gr.Name == groupName);
        }

        public void ClearUserGroupsByUserId(string userId)
        {
            RemoveRolesByUserId(userId);
            ApplicationUser user = _context.Users.Find(userId);
            user.Groups.Clear();
            _context.SaveChanges();
        }

        public void AddUserToGroup(string userId, int groupId)
        {
            Group group = _context.Groups.Find(groupId);
            ApplicationUser user = _context.Users.Find(userId);

            var userGroup = new ApplicationUserGroup
            {
                Group = group,
                GroupId = group.Id,
                User = user,
                UserId = user.Id
            };

            foreach (ApplicationRoleGroup role in group.ApplicationRoleGroups)
            {
                _userManager.AddToRole(userId, role.Role.Name);
            }
            user.Groups.Add(userGroup);
            _context.SaveChanges();
        }

        // remove role by Group id on table [ApplicationRoleGroups]
        public void RemoveGroupRolesByGroupId(int groupId)
        {
            Group group = _context.Groups.Find(groupId);
            var userlistFromGroup = _context.Users.Where(u => u.Groups.Any(g => g.GroupId == groupId));
            
            foreach (ApplicationRoleGroup roleGroup in group.ApplicationRoleGroups)
            {
                foreach (ApplicationUser user in userlistFromGroup)
                {
                    // Is the user a member of any other groups with this role?
                    int groupsWithRole = user.Groups.Count(g => g.Group.ApplicationRoleGroups
                        .Any(r => r.RoleId == roleGroup.RoleId));

                    // This will be 1 if the current group is the only one:
                    if (groupsWithRole == 1)
                    {
                        // delete user from aspNetUserRoles
                        _userManager.RemoveFromRole(user.Id, roleGroup.Role.Name);
                    }
                }
            }
            group.ApplicationRoleGroups.Clear();
            _context.SaveChanges();
        }

        public void CreateApplicationRoleGroup(int groupId, string roleName)
        {
            Group group = _context.Groups.Find(groupId);
            ApplicationRole role = _context.Roles.First(r => r.Name == roleName);

            var newgroupRole = new ApplicationRoleGroup
            {
                GroupId = group.Id,
                Group = group,
                RoleId = role.Id,
                Role = role
            };

            // checking ! make sure the [ApplicationRoleGroups] is not exist
            if (!group.ApplicationRoleGroups.Contains(newgroupRole))
            {
                group.ApplicationRoleGroups.Add(newgroupRole);
                _context.SaveChanges();
            }

            // Add all of the users in this group to the new role:
            //IQueryable<ApplicationUser> groupUsers = _context.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));

            foreach (ApplicationUser user in _context.Users.Where(u => u.Groups.Any(g => g.GroupId == groupId)).ToList())
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    _userManager.AddToRole(user.Id, roleName);//AddUserToRole(user.Id, role.Name);
                }
            }
        }

        public void DeleteGroup(int groupId)
        {
            Group group = _context.Groups.Find(groupId);

            // Clear the roles from the group:
            RemoveGroupRolesByGroupId(groupId);
            _context.Groups.Remove(group);
            _context.SaveChanges();
        }
    }

    [Serializable]
    public class GroupExistsException : Exception
    {
        public GroupExistsException()
        {
        }

        public GroupExistsException(string message) : base(message)
        {
        }

        public GroupExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GroupExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}