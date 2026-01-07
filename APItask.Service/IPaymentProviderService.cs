using APItask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IPaymentProviderService
    {
        Task<PaymentResult> InitializePayment(PaymentRequest request, string reference);
        Task<PaymentResult> VerifyPayment(string reference);
        string GetProviderName();
    }
}
