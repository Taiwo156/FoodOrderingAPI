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
        Task<Payment> SavePaymentAsync (Payment payment);
        Task<Payment> GetVerifiedPaymentByReferenceAsync(string reference);
        Task<List<Payment>> GetPaymentsAsync(
            string email = null,
            string status = null,
            int pageSize = 50
            );
        Task<bool> UpdateVerifiedPaymentStatusAsync(string reference, string status);
    }
}