using System;
using System.Threading.Tasks;
using APItask.Core.Models;
using APItask.Data;
using APItask.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace APItask.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(
            ILogger<PaymentService> logger,
            IConfiguration configuration,
            IPaymentRepository paymentRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResult> InitializePayment(PaymentRequest request)
        {
            try
            {
                var reference = await _paymentRepository.GeneratePaymentReference();
                var paymentAttempt = new PaymentAttempt
                {
                    Reference = reference,
                    Amount = request.Amount,
                    Currency = request.Currency,
                    Method = request.Method,
                    Email = request.Email,
                    Phone = request.Phone,
                    Status = "pending" // Start as pending
                };

                await _paymentRepository.SavePaymentAttempt(paymentAttempt);
                _logger.LogInformation($"Mock Payment {reference} saved to database. Method: {request.Method}");

                // Simple fire-and-forget simulation (for demo purposes only)
                _ = SimulateProviderWebhook(reference);

                // --- MOCK RESPONSES FOR SCHOOL PROJECT ---
                var mockResponse = request.Method.ToLower() switch
                {
                    "bank-transfer" => new PaymentResult
                    {
                        Success = true,
                        Message = "Bank transfer details generated.",
                        Reference = reference,
                        BankName = "Opay",
                        AccountNumber = "9836521965",
                        AccountName = "Tee Foods",
                        Amount = request.Amount
                    },
                    "opay" => new PaymentResult
                    {
                        Success = true,
                        Message = "Redirect to OPay to complete payment. (This is a mock - no redirection will happen)",
                        Reference = reference,
                        AuthorizationUrl = $"https://your-mock-server.com/opay/checkout?ref={reference}" // This is a fake URL
                    },
                    "credit-card" => new PaymentResult
                    {
                        Success = true,
                        Message = "Card payment request received. (This is a mock - no real charge was made)",
                        Reference = reference
                    },
                    "ussd" => new PaymentResult
                    {
                        Success = true,
                        Message = "Please dial *723*Amount# on your phone. (Mock Instruction)",
                        Reference = reference
                    },
                    "paga" => new PaymentResult
                    {
                        Success = true,
                        Message = "Redirect to Paga to complete payment. (This is a mock)",
                        Reference = reference,
                        AuthorizationUrl = $"https://your-mock-server.com/paga/checkout?ref={reference}" // Fake URL
                    },
                    "cash-delivery" => new PaymentResult
                    {
                        Success = true,
                        Message = "Order confirmed. You will pay with cash on delivery.",
                        Reference = reference
                    },
                    _ => new PaymentResult
                    {
                        Success = false,
                        Message = $"Payment method '{request.Method}' is not supported in this mock project."
                    }
                };

                return mockResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment initialization failed");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Payment failed: {ex.Message}"
                };
            }
        }
        // Add this method to your PaymentService class
        private async Task SimulateProviderWebhook(string reference)
        {
            try
            {
                // Wait a random time between 15 and 45 seconds to simulate processing
                var delaySeconds = new Random().Next(15, 46);
                await Task.Delay(delaySeconds * 1000);

                // Then "verify" the payment by updating its status in the database
                var payment = await _paymentRepository.GetPaymentByReference(reference);
                if (payment != null && payment.Status == "pending")
                {
                    // Simulate a successful payment most of the time, but occasional failure
                    var success = new Random().NextDouble() > 0.2; // 80% success rate
                    var newStatus = success ? "completed" : "failed";

                    await _paymentRepository.UpdatePaymentStatus(reference, newStatus);
                    _logger.LogInformation($"SIMULATED WEBHOOK: Payment {reference} marked as {newStatus} after {delaySeconds} seconds.");
                }
            }
            catch (Exception ex)
            {
                // Log any errors in the simulation, but don't let it crash the main request
                _logger.LogError(ex, $"Error in simulated webhook for reference {reference}");
            }
        }

        // ------- SPECIFIC PAYMENT PROCESSING METHODS -------

        private async Task<PaymentResult> ProcessBankTransfer(PaymentRequest request, string reference)
        {
            // For bank transfer, we return the details immediately
            var bankDetails = await GetBankTransferDetails(request.Amount, request.Currency);

            return new PaymentResult
            {
                Success = true,
                Message = "Bank transfer details generated.",
                Reference = reference,
                BankName = bankDetails.BankName,
                AccountNumber = bankDetails.AccountNumber,
                AccountName = bankDetails.AccountName,
                Amount = bankDetails.Amount
            };
        }

        private async Task<PaymentResult> ProcessOPay(PaymentRequest request, string reference)
        {
            // TODO: Replace with actual OPay API integration
            // This usually returns a URL to redirect the user to
            await Task.Delay(100); // Simulate API call

            return new PaymentResult
            {
                Success = true,
                Message = "Redirect to OPay to complete payment.",
                Reference = reference,
                AuthorizationUrl = $"https://opay-checkout-url.com?tx_ref={reference}" // Example
            };
        }

        private async Task<PaymentResult> ProcessCardPayment(PaymentRequest request, string reference)
        {
            // TODO: Integrate with your new card payment provider API here.
            // This is a mock implementation.
            await Task.Delay(100);

            return new PaymentResult
            {
                Success = true,
                Message = "Card payment processed successfully.",
                Reference = reference
            };
        }

        private PaymentResult ProcessUssdPayment(PaymentRequest request, string reference)
        {
            // For USSD, provide instructions
            return new PaymentResult
            {
                Success = true,
                Message = "Please use the USSD code *723*Amount# to complete payment.",
                Reference = reference
            };
        }

        private async Task<PaymentResult> ProcessPaga(PaymentRequest request, string reference)
        {
            // TODO: Integrate with Paga API
            await Task.Delay(100);

            return new PaymentResult
            {
                Success = true,
                Message = "Redirect to Paga to complete payment.",
                Reference = reference,
                AuthorizationUrl = $"https://mypaga.com/pay?reference={reference}"
            };
        }

        private PaymentResult ProcessCashOnDelivery(PaymentRequest request, string reference)
        {
            // Nothing to process for COD
            return new PaymentResult
            {
                Success = true,
                Message = "Order placed successfully. You will pay on delivery.",
                Reference = reference
            };
        }

        // 

        public async Task<PaymentResult> VerifyPayment(string reference)
        {
            var payment = await _paymentRepository.GetPaymentByReference(reference);

            if (payment == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = "Payment not found.",
                    Reference = reference
                };
            }

            // --- MOCK LOGIC FOR SCHOOL PROJECT ---
            // Simulate different real-world scenarios based on the payment's age or status
            var timeSinceCreation = DateTime.UtcNow - payment.CreatedAt;

            if (payment.Status == "completed")
            {
                // If it's already completed, just return success
                return new PaymentResult
                {
                    Success = true,
                    Message = "Payment already verified successfully.",
                    Reference = reference
                };
            }
            else if (timeSinceCreation.TotalSeconds > 30)
            {
                // If the payment was created more than 30 seconds ago, simulate a successful payment
                await _paymentRepository.UpdatePaymentStatus(reference, "completed");
                return new PaymentResult
                {
                    Success = true,
                    Message = "Payment verified successfully!",
                    Reference = reference
                };
            }
            else if (timeSinceCreation.TotalSeconds > 10)
            {
                // If it's between 10 and 30 seconds old, simulate it being pending
                return new PaymentResult
                {
                    Success = false,
                    Message = "Payment is still processing. Please wait.",
                    Reference = reference
                };
            }
            else
            {
                // If it's very new, simulate it failing (e.g., user insufficient funds, network error)
                // Let's only fail some of them to be realistic
                var shouldFail = new Random().NextDouble() > 0.7; // 30% chance of failure

                if (shouldFail)
                {
                    await _paymentRepository.UpdatePaymentStatus(reference, "failed");
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Payment failed. Please try a different method.",
                        Reference = reference
                    };
                }
                else
                {
                    // Otherwise, it's still pending
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Payment is pending.",
                        Reference = reference
                    };
                }
            }
        }

        public async Task<BankTransferDetails> GetBankTransferDetails(decimal amount, string currency)
        {
            var reference = await _paymentRepository.GeneratePaymentReference();

            return new BankTransferDetails
            {
                BankName = "Opay",
                AccountNumber = "9836521965",
                AccountName = "Tee Foods",
                Amount = amount,
                Currency = currency,
                
            };
        }
    }
}