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
            return $"TEE{timestamp}{uniqueId}";
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

        public async Task<Payment> SavePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetVerifiedPaymentByReferenceAsync(string reference)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Reference == reference);
        }
        public async Task<List<Payment>> GetPaymentsAsync(
            string email = null,
            string status = null,
            int pageSize = 50
            )
        {
            //start with all payments
            var query = _context.Payments.AsQueryable();

            //filter by email if provided
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(p => p.Email == email);
            }

            //filter by status if provided
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            //Order by newest first , length limit
            return await query.OrderByDescending(p => p.CreatedAt).Take(pageSize).ToListAsync();
        }
        public async Task<bool> UpdateVerifiedPaymentStatusAsync(string reference, string status)
        {
            // find payment
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Reference == reference);

            if (payment == null)
                return false;

            //update status
            payment.Status = status;
            payment.UpdatedAt = DateTime.UtcNow;

            //set VerifiedAt timestamp if payment is suucessful
            if (status == "success")
            {
                payment.VerifiedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}