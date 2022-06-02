using OneRegister.Data.Entities.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public interface IInquirer
    {
        bool IsEligible(InquiryType inquiryType);
        void Inquiry(InquiryTask inquiryTask);
    }
}
