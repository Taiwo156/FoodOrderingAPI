// APItask.Data/PaymentRepository.cs
using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EssentialProductsDbContext _context;

        public PaymentRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<string> GeneratePaymentReference()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHmmss");
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            return $"TEE{ timestamp}{ uniqueId}";
        }

        public async Task SavePaymentAttempt(PaymentAttempt paymentAttempt)
        {
            _context.PaymentAttempts.Add(paymentAttempt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentStatus(string reference, string status)
        {
            var payment = await _context.PaymentAttempts
                .FirstOrDefaultAsync(p => p.Reference == reference);

            if (payment != null)
            {
                payment.Status = status;
                payment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaymentAttempt> GetPaymentByReference(string reference)
        {
            return await _context.PaymentAttempts
                .FirstOrDefaultAsync(p => p.Reference == reference);
        }
    }
}