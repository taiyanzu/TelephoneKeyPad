using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TelephoneKeyPad.Domain;
using TelephoneKeyPad.Web.Models;

namespace TelephoneKeyPad.Web.Controllers
{
    public class DemoController : Controller
    {
        private const int PAGE_SIZE = 10;
        private ICombinationGenerator _generator;
        
        public DemoController(ICombinationGenerator combinationGenerator)
        {
            _generator = combinationGenerator;
        }

        public ActionResult Index()
        {
            return View(new CombinationsVM());
        }

        [HttpPost]
        public ActionResult Index(CombinationsVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var updatedModel = GetPagedResult(model.PhoneNumber, 0);
                return View(updatedModel);
            }
        }

        public ActionResult Page(string phoneNumber, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var model = GetPagedResult(phoneNumber, currentPageIndex);
            return PartialView("_PagedItems", model);
        }

        private CombinationsVM GetPagedResult(string phoneNumber, int pageIndex)
        {
            var total = _generator.TotalItemCount(phoneNumber);
            IEnumerable<string> source = _generator.Generate(phoneNumber);
            return new CombinationsVM
            {
                PhoneNumber = phoneNumber,
                TotalItemCount = total,
                PageSize = PAGE_SIZE,
                PageIndex = pageIndex,
                PagedItems = source.Skip(pageIndex * PAGE_SIZE).Take(PAGE_SIZE).ToArray()
            };
        }
    }
}
