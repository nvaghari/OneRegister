using Microsoft.AspNetCore.Mvc;
using OneRegister.Domain.Services.MerchantRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.ViewComponents.MerchantCommission
{
    public class MerchantCommissionViewComponent : ViewComponent
    {
        private readonly MerchantService _merchantService;

        public MerchantCommissionViewComponent(MerchantService merchantService)
        {
            _merchantService = merchantService;
        }
        public IViewComponentResult Invoke(Guid? merchantId)
        {
            var model = new VCM_MerchantCommission
            {
                HasCommission = _merchantService.HasCommission(merchantId)
            };
            return View(model);
        }
    }
}
