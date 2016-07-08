using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;
using EfDemo.Presentation.Web.DefaultSite.CodeBase;
using EfDemo.Presentation.Web.DefaultSite.Models;

namespace EfDemo.Presentation.Web.DefaultSite.Controllers
{
    [Authorize()]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new ArgumentNullException(nameof(categoryRepository));
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index() => View();

        public ActionResult NewCategory() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewCategory(CategoryModel category)
        {
            if (!ModelState.IsValid) return View(category);
            category.CreatedBy = GetCurrentUser();
            await _categoryRepository.SaveCategoryAsync((Category)category).ConfigureAwait(false);
            return View(category);
        }
    }
}