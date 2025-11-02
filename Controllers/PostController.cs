using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.ViewModels;

namespace ForumApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Yorum içeriği boş olamaz.";
                return RedirectToAction("Details", new { id = postId });
            }

            if (content.Length > 1000)
            {
                TempData["ErrorMessage"] = "Yorum en fazla 1000 karakter olabilir.";
                return RedirectToAction("Details", new { id = postId });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var comment = new Comment
            {
                Content = content,
                PostId = postId,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yorumunuz başarıyla eklendi.";
            return RedirectToAction("Details", new { id = postId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int postId)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (comment.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
            return RedirectToAction("Details", new { id = postId });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (post.UserId != userId)
            {
                return Forbid();
            }

            var model = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _context.Posts.FindAsync(model.Id);

                if (post == null)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                if (post.UserId != userId)
                {
                    return Forbid();
                }

                post.Title = model.Title;
                post.Content = model.Content;
                post.UpdatedDate = DateTime.UtcNow;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = post.Id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (post.UserId != userId)
            {
                return Forbid();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            var userId = _userManager.GetUserId(User);

            var posts = await _context.Posts
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(posts);
        }
    }
}
