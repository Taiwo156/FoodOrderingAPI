using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APItask.Service;
using APItask.Core.Models;

namespace APItask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var result = await _paymentService.InitializePayment(request);
            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyRequest request)
        {
            var result = await _paymentService.VerifyPayment(request.Reference);
            return Ok(result);
        }

        [HttpGet("bank-details")]
        public async Task<IActionResult> GetBankDetails([FromQuery] decimal amount, [FromQuery] string currency = "NGN")
        {
            var details = await _paymentService.GetBankTransferDetails(amount, currency);
            return Ok(details);
        }

        [HttpGet("methods")]
        public IActionResult GetPaymentMethods()
        {
            var methods = new[]
            {
                new { id = "credit-card", name = "Credit/Debit Card", icon = "credit_card" },
                new { id = "bank-transfer", name = "Bank Transfer", icon = "account_balance" },
                new { id = "ussd", name = "USSD", icon = "phone_android" },
                new { id = "opay", name = "OPay", icon = "account_balance_wallet" },
                new { id = "paga", name = "Paga", icon = "mobile_friendly" },
                new { id = "cash-delivery", name = "Cash on Delivery", icon = "money" }
            };

            return Ok(methods);
        }
    }

    public class VerifyRequest
    {
        public string Reference { get; set; }
    }
}