using Microsoft.AspNetCore.Mvc;
using OneRegister.Domain.Model.Shared.Billboard;
using System;

namespace OneRegister.Web.Controllers
{
    public class BillboardController : Controller
    {
        [HttpGet]
        public IActionResult Index(string t)
        {
            var model = new BillboardModel();
            if (Enum.TryParse(typeof(BillboardType), t, out object type))
            {
                model.Type = (BillboardType)type;
            }
            else
            {
                model.Type = BillboardType.NotFound;
            }
            model.Text = TempData["bText"]?.ToString();
            model.Url = TempData["bUrl"]?.ToString();
            var baseUrl = Url.Content("~/");
            switch (model.Type)
            {
                case BillboardType.Success:
                    model.Title = "Awesome, You did it Successfully!";
                    model.Photo = baseUrl + "pic/billboard/highFive.png";
                    break;
                case BillboardType.Failure:
                    model.Title = "Mmm.. Something went Wrong!";
                    model.Photo = baseUrl + "pic/billboard/QA_engineers.png";
                    break;
                default:
                    model.Title = "Oops! so sorry, We couldn't find it!";
                    model.Photo = baseUrl + "pic/billboard/page_not_found.png";
                    break;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(BillboardModel model)
        {
            if (model == null && TempData["model"] != null)
            {
                model = (BillboardModel)TempData["model"];
            }
            var baseUrl = Url.Content("~/");
            switch (model.Type)
            {
                case BillboardType.Success:
                    model.Title = "Awesome, You did it Successfully!";
                    model.Photo = baseUrl + "pic/billboard/highFive.png";
                    break;
                case BillboardType.Failure:
                    model.Title = "Mmm.. Something went Wrong!";
                    model.Photo = baseUrl + "pic/billboard/QA_engineers.png";
                    break;
                default:
                    model.Title = "Oops! so sorry, We couldn't find it!";
                    model.Photo = baseUrl + "pic/billboard/page_not_found.png";
                    break;
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
