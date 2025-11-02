using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumApp.Data;
using ForumApp.Models;

namespace ForumApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Post başarıyla silindi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Users");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (isInRole)
            {
                TempData["ErrorMessage"] = "Kullanıcı zaten admin.";
                return RedirectToAction("Users");
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"{user.UserName} başarıyla admin yapıldı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Admin yapma işlemi başarısız oldu.";
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Users");
            }

            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId)
            {
                TempData["ErrorMessage"] = "Kendinizi admin olmaktan çıkaramazsınız.";
                return RedirectToAction("Users");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                TempData["ErrorMessage"] = "Kullanıcı zaten admin değil.";
                return RedirectToAction("Users");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"{user.UserName} admin olmaktan çıkarıldı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Admin çıkarma işlemi başarısız oldu.";
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Users");
            }

            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId)
            {
                TempData["ErrorMessage"] = "Kendi hesabınızı silemezsiniz.";
                return RedirectToAction("Users");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"{user.UserName} başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı silme işlemi başarısız oldu.";
            }

            return RedirectToAction("Users");
        }
    }
}
