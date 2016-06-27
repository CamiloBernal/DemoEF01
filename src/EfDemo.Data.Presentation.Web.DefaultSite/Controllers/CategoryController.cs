using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;
using EfDemo.Data.Presentation.Web.DefaultSite.CodeBase;
using EfDemo.Data.Presentation.Web.DefaultSite.Models;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Controllers
{
    public class CategoryController:BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new ArgumentNullException(nameof(categoryRepository));
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index() => View();

        public ActionResult NewCategory() => View();

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.SaveCategoryAsync((Category)category).ConfigureAwait(false);
            }
            return View(category);
        }
    }
}