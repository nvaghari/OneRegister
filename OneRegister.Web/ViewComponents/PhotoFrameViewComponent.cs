using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.ViewComponents
{
    public class PhotoFrameViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string photoId)
        {
            return View();
        }
    }
}
