using APItask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IPaymentService
    {
        Task<PaymentResult> InitializePayment(PaymentRequest request);
        Task<PaymentResult> VerifyPayment(string reference);
        Task<BankTransferDetails> GetBankTransferDetails(decimal amount, string currency);
    }
}
