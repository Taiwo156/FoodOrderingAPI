using APItask.Core.Models;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IPaymentRepository
    {
        Task<string> GeneratePaymentReference();
        Task SavePaymentAttempt(PaymentAttempt paymentAttempt);
        Task UpdatePaymentStatus(string reference, string status);
        Task<PaymentAttempt> GetPaymentByReference(string reference);
    }
}