using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelephoneKeyPad.Web.Models;

namespace TelephoneKeyPad.Web.Controllers
{
    public class DemoController : Controller
    {
        private CombinationsVM exampleCombinations = new CombinationsVM
        {
            OriginalNumber = "2223334567",
            ItemCount = 1000,
            PageCount = 100,
            PageSize = 10,
            PageIndex = 0,
            PagedItems = Enumerable.Repeat(Guid.NewGuid().ToString(), 10).ToArray()
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
                return View(exampleCombinations);
            }
            
        }
    }
}