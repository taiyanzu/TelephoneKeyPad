using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelephoneKeyPad.Domain;
using TelephoneKeyPad.Web.Models;

namespace TelephoneKeyPad.Web.Controllers
{
    public class DemoController : Controller
    {
        private ICombinationGenerator _combinationGenerator;
        
        public DemoController(ICombinationGenerator combinationGenerator)
        {
            _combinationGenerator = combinationGenerator;
        }

        private CombinationsVM fakedCombinations => new CombinationsVM
        {
            OriginalNumber = "2223334567",
            TotalItemCount = _combinationGenerator.TotalItemCount(),
            PageSize = 10,
            PageIndex = 0,
            PagedItems = _combinationGenerator.GetPageItems().ToArray()
        };

        // GET: Demo
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
                // fake the generation
                return View(fakedCombinations);
            }
        }

        public ActionResult Page(string phoneNumber, int? page)
        {
            return View(fakedCombinations);
        }
    }
}
