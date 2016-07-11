using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Threading.Tasks;
using System.Web.Mvc;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;
using EfDemo.Crosscutting.IoC.Mvc;
using EfDemo.Presentation.Web.DefaultSite.Models;

namespace EfDemo.Presentation.Web.DefaultSite.Controllers
{
    public class ConfidentialUserInfoController : Controller
    {
        private readonly IConfidentialUserInfoRepository _confidentialUserInfoRepository;

        public ConfidentialUserInfoController(IConfidentialUserInfoRepository confidentialUserInfoRepository)
        {
            if (confidentialUserInfoRepository == null)
                throw new ArgumentNullException(nameof(confidentialUserInfoRepository));
            _confidentialUserInfoRepository = confidentialUserInfoRepository;
        }


        public async Task<ActionResult> Index()
        {
            var data = await _confidentialUserInfoRepository.GetUserConfidentialInfoAsync().ConfigureAwait(false);
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ConfidentialUserInfoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _confidentialUserInfoRepository.RegisterConfidentialInfoAsync((ConfidentialUserInfo)model).ConfigureAwait(false);
            return View();
        }
    }
}