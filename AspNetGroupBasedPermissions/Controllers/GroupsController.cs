using AspNetGroupBasedPermissions.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AspNetGroupBasedPermissions.ViewModels;

namespace AspNetGroupBasedPermissions.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IdentityManager _imanager;

        public GroupsController()
        {
            _db = new ApplicationDbContext();
            _imanager = new IdentityManager();
        }


        [Authorize(Roles = "Admin, CanEditGroup, CanEditUser")]
        public ActionResult Index()
        {
            return View(_db.Groups.ToList());
        }


        [Authorize(Roles = "Admin, CanEditGroup, CanEditUser")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = _db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                _db.Groups.Add(group);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = _db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(group).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = _db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "Admin, CanEditGroup")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _imanager.DeleteGroup(id);
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Admin, CanEditGroup")]
        public ActionResult GroupRoles(int id)
        {
            var group = _db.Groups.Find(id);
            var groupList = group.Roles.ToList();

            var roleList = (_db.Roles.ToList()
                .Select(u => new SelectRoleEditorViewModel()
                {
                    Description = u.Description,
                    RoleName = u.Name,
                    Selected = groupList.Any(x => x.Role.Name == u.Name)
                })).ToList();

            var model = new SelectGroupRolesViewModel()
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Roles = roleList
            };
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, CanEditGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult GroupRoles(SelectGroupRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = _db.Groups.Find(model.GroupId);
                _imanager.RemoveGroupRolesByGroupId(model.GroupId);

                // Add each selected role to this group:
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        _imanager.CreateApplicationRoleGroup(group.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
