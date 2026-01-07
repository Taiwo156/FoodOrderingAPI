using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APItask.Service;
using APItask.Core.Models;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using APItask.Data;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Text.Json.Serialization;

namespace APItask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("test-paystack")]
        public async Task<ActionResult<PaymentResult>> TestPaystack([FromBody] PaymentRequest request)
        {
            try
            {
                Console.WriteLine($"=== TestPaystack STARTED ===");
                Console.WriteLine($"Request received: Amount={request.Amount}, Email={request.Email}, Method={request.Method}");
                // Generate reference
                var reference = $"TEST_{DateTime.UtcNow:yyyyMMddHHmmss}";
                Console.WriteLine($"Generated reference: {reference}");

                // Get Paystack service
                var paystackService = HttpContext.RequestServices
                    .GetRequiredService<IPaymentProviderService>();

                Console.WriteLine($"Paystack service retrieved: {paystackService.GetType().Name}");

                // Log configuration (be careful with secrets in production!)
                var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var secretKey = config["PaymentProviders:Paystack:SecretKey"];
                Console.WriteLine($"SecretKey configured: {!string.IsNullOrEmpty(secretKey)}");
                Console.WriteLine($"BaseUrl: {config["PaymentProviders:Paystack:BaseUrl"]}");
                Console.WriteLine($"CallbackUrl: {config["PaymentProviders:Paystack:CallbackUrl"]}");

                // Initialize payment
                Console.WriteLine($"Calling InitializePayment...");
                var result = await paystackService.InitializePayment(request, reference);

                Console.WriteLine($"=== TestPaystack COMPLETE ===");
                Console.WriteLine($"Success: {result.Success}");
                Console.WriteLine($"Message: {result.Message}");
                Console.WriteLine($"Reference: {result.Reference}");
                Console.WriteLine($"Auth URL: {result.AuthorizationUrl}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log to console for debugging
                Console.WriteLine($"=== EXCEPTION in TestPaystack ===");
                Console.WriteLine($"Type: {ex.GetType().FullName}");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return BadRequest(new
                {
                    error = ex.Message,
                    type = ex.GetType().Name,
                    details = ex.StackTrace,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("verify")]
        public async Task<ActionResult<PaymentResult>> VerifyPayment([FromBody] VerifyRequest request)
        {
            try
            {
                //log the information
                _logger.LogInformation("===VERIFY PAYMENT STARTED===");
                _logger.LogInformation($"Reference to verify: {request.Reference}");

                var paystackService = HttpContext.RequestServices.GetRequiredService<IPaymentProviderService>();
                var result = await paystackService.VerifyPayment(request.Reference);

                _logger.LogInformation("===VERIFY PAYMENT CPMPLETED===");
                _logger.LogInformation($"Success{result.Success}, Amount: {result.Amount}");

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error Verifying /payment");
                return BadRequest(new {error = ex.Message});
            }
        }

        [HttpPost("paystack-webhook")]
        public async Task<IActionResult> PaystackWebhook()
        {
            try
            {
                _logger.LogInformation("===WEBHOOK RECEIVED FROM PAYSTACK===");


                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                _logger.LogInformation($"Webhook body length: {body.Length} chars");

                //Get Paystack Signature
                var signature = Request.Headers["x-paystack-signature"].ToString();

                if (string.IsNullOrEmpty(signature))
                {
                    _logger.LogInformation("No signature in webhook");
                    return BadRequest("No signature");
                }

                //Verify Paystack Signature
                var isValid = VerifyPaystackSignature(body, signature);

                if (!isValid)
                {
                    _logger.LogInformation("Invalid Webhook Signature");
                    return BadRequest("Invalid signature");
                }

                _logger.LogInformation("Signature verified!");

                //parse webhook data
                dynamic webhookData = JsonConvert.DeserializeObject(body);

                // check event type
                string eventType = webhookData.@event;
                _logger.LogInformation($"Event type: {eventType}");

                // handle successful charge
                if (eventType == "charge.success")
                {
                    string reference = webhookData.data.reference;
                    decimal amount = webhookData.data.amount / 100m;
                    string status = webhookData.data.status;
                    string email = webhookData.data.customer?.email ?? "unknown@email.com";
                    string paymentMethod = webhookData.data.channel ?? "card";


                    _logger.LogInformation($" Payment Successful! ref: {reference}, Amount: ₦{amount}");

                    //save to database
                    var paymentRepo = HttpContext.RequestServices.GetRequiredService<IPaymentRepository>();

                    //check if it already exist
                    var existingPayment = await paymentRepo.GetVerifiedPaymentByReferenceAsync(reference);

                    if(existingPayment == null)
                    {
                        var payment = new Payment
                        {
                            Reference = reference,
                            Amount = amount,
                            Currency = "NGN",
                            Email = email,
                            Status = "success",
                            PaymentMethod = paymentMethod,
                            Provider = "Paystack",
                            ProviderResponse = body,
                            PaidAt = DateTime.UtcNow,
                            VerifiedAt = DateTime.UtcNow,
                            WebhookReceived = true,
                            WebhookReceivedAt = DateTime.UtcNow
                        };
                        await paymentRepo.SavePaymentAsync(payment);

                        _logger.LogInformation( $"Payment {reference} saved to database");
                    }
                    else
                    {
                        _logger.LogInformation($"Payment {reference} already exist in database");
                    }

                    return Ok();
                }
                _logger.LogInformation($"Unhandled event: {eventType}");
                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error Processing webhook");
                return StatusCode(500, "Webhook failed");
            }
        }



        private bool VerifyPaystackSignature(string body, string signature)
        {
            try
            {
                var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

                var secretKey = config["PaymentProviders:Paystack:SecretKey"];

                if (string.IsNullOrEmpty(secretKey))
                {
                    _logger.LogError("Secret Key not found");
                    return false;
                }

                //calculate expected signature
                using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
                var computed = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return computed == signature.ToLower();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error verifying signature");
                return false;
            }
        }

        [HttpGet("paystack-callback")]
        public async Task<IActionResult> PaystackCallback([FromQuery] string reference, [FromQuery] string trxref)
        {
            try
            {
                _logger.LogInformation("=== CALLBACK RECEIVED ===");
                _logger.LogInformation($"Reference: {reference}, Trxref: {trxref}");

                //Use the reference from query parameters
                var paymentRef = reference ?? trxref;

                if (string.IsNullOrEmpty(paymentRef))
                {
                    return BadRequest("No payment reference provided");
                }

                //verify payments with paystack
                var paymentService = HttpContext.RequestServices.GetRequiredService<IPaymentProviderService>();

                var result = await paymentService.VerifyPayment(paymentRef);

                if (result.Success) 
                {

                    return Content($@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Payment Successful</title>
                    <style>
                        body {{ 
                            font-family: Arial, sans-serif; 
                            text-align: center; 
                            padding: 50px;
                            background: #f0f0f0;
                        }}
                        .success-box {{
                            background: white;
                            padding: 40px;
                            border-radius: 10px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                            max-width: 500px;
                            margin: 0 auto;
                        }}
                        .checkmark {{ 
                            color: #4CAF50; 
                            font-size: 60px; 
                        }}
                        h1 {{ color: #333; }}
                        .amount {{ 
                            font-size: 32px; 
                            color: #4CAF50; 
                            font-weight: bold;
                            margin: 20px 0;
                        }}
                        .reference {{
                            background: #f5f5f5;
                            padding: 10px;
                            border-radius: 5px;
                            margin: 20px 0;
                            font-family: monospace;
                        }}
                    </style>
                </head>
                <body>
                    <div class='success-box'>
                        <div class='checkmark'>✓</div>
                        <h1>Payment Successful!</h1>
                        <div class='amount'>₦{result.Amount:N2}</div>
                        <p>Your payment has been processed successfully.</p>
                        <div class='reference'>
                            Reference: {paymentRef}
                        </div>
                        <p style='color: #666; margin-top: 30px;'>
                            You can close this window now.
                        </p>
                    </div>
                </body>
                </html>
            ", "text/html");
                }
                else
                {
                    // Payment failed
                    return Content($@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>Payment Failed</title>
                        <style>
                            body {{ 
                                font-family: Arial, sans-serif; 
                                text-align: center; 
                                padding: 50px;
                                background: #f0f0f0;
                            }}
                            .error-box {{
                                background: white;
                                padding: 40px;
                                border-radius: 10px;
                                box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                                max-width: 500px;
                                margin: 0 auto;
                            }}
                            .error-icon {{ 
                                color: #f44336; 
                                font-size: 60px; 
                            }}
                            h1 {{ color: #333; }}
                        </style>
                    </head>
                    <body>
                        <div class='error-box'>
                            <div class='error-icon'>✕</div>
                            <h1>Payment Failed</h1>
                            <p>{result.Message}</p>
                            <p style='color: #666; margin-top: 30px;'>
                                Please try again or contact support.
                            </p>
                        </div>
                    </body>
                    </html>
                ", "text/html");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in callback");
                    return StatusCode(500, "An error occured processing your payment");
                }
        }

        [HttpPost("flutterwave-webhook")]
        public async Task<IActionResult> FlutterwaveWebhook()
        {
            try
            {
                _logger.LogInformation("=== WEBHOOK RECEIVED FROM FLUTTERWAVE ===");

                //read webhook body
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                _logger.LogInformation($"Webhook body length: {body.Length} chars");

                // get flutterwave signature
                var signature = Request.Headers["verif-hash"].ToString();

                if (string.IsNullOrEmpty(signature))
                {
                    _logger.LogInformation("No signature in Flutterwave webhook");
                    return BadRequest("No signature");
                }

                // verify flutterwave signature
                var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var secretHash = config["PaymentProviders:Flutterwave:SecretKey"];

                if (signature != secretHash)
                {
                    _logger.LogWarning("Invalid Flutterwave webhook signature");
                    return BadRequest("Invalid signature");
                }

                _logger.LogInformation("Flutterwave signature verified!");

                // parse webhook data
                dynamic webhookData = JsonConvert.DeserializeObject(body);

                //check event tyoe
                string eventType = webhookData.@event;
                _logger.LogInformation($"Event type: {eventType}");

                // handle successful charge
                if (eventType == "charge.completed")
                {
                    string status = webhookData.data.status;
                    if (status == "successful")
                    {
                        string reference = webhookData.data.tx_ref;
                        decimal amount = webhookData.data.amount;
                        string email = webhookData.data.customer?.email ?? "unknown@email.com";
                        string paymentMethod = webhookData.data.payment_type ?? "card";

                        _logger.LogInformation($"Payment Successful! ref: {reference}, Amount ₦{amount}");

                        // save to database
                        var paymentRepo = HttpContext.RequestServices.GetRequiredService<IPaymentRepository>();

                        // check if already exists (prevent duplicates)
                        var existingPayment = await paymentRepo.GetVerifiedPaymentByReferenceAsync(reference);

                        if(existingPayment == null)
                        {
                            _logger.LogInformation($"Saving payment {reference} to database");

                            var payment = new Payment
                            {
                                Reference = reference,
                                Amount = amount,
                                Currency = "NGN",
                                Email = email,
                                Status = "success",
                                PaymentMethod = paymentMethod,
                                Provider = "Flutterwave",
                                ProviderResponse = body,
                                PaidAt = DateTime.UtcNow,
                                VerifiedAt = DateTime.UtcNow,
                                WebhookReceived = true,
                                WebhookReceivedAt = DateTime.UtcNow,
                                InitializedAt = DateTime.UtcNow
                            };
                            await paymentRepo.SavePaymentAsync(payment);

                            _logger.LogInformation($"Payment {reference} saved to database");
                        }
                        else
                        {
                            _logger.LogInformation($"Payment {reference} already exists in database");
                        }

                        return Ok();
                    }
                }
                _logger.LogInformation($"Unhandled Flutterwave event: {eventType}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Flutterwave webhook");
                return StatusCode(500, "Webhook failed");
            }
        }

        [HttpGet("flutterwave-callback")]
        public async Task<IActionResult> FlutterwaveCallback([FromQuery] string status,
            [FromQuery] string tx_ref, [FromQuery] string transaction_id)
        {
            try
            {
                _logger.LogInformation("=== FLUTTERWAVE CALLBACK RECEIVED ===");
                _logger.LogInformation($"Status: {status}, Reference: {tx_ref}, Transaction ID: {transaction_id}");

                if (string.IsNullOrEmpty(tx_ref))
                {
                    return BadRequest("No payment reference provided");
                }

                // verify payment with flutterwave
                var flutterwaveService = HttpContext.RequestServices.GetRequiredService<FlutterwaveService>();
                var result = await flutterwaveService.VerifyPayment(tx_ref);

                if (result.Success && status == "successful")
                {
                    // Payment successful! Show success page
                    return Content($@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Payment Successful</title>
                    <style>
                        body {{ 
                            font-family: Arial, sans-serif; 
                            text-align: center; 
                            padding: 50px;
                            background: #f0f0f0;
                        }}
                        .success-box {{
                            background: white;
                            padding: 40px;
                            border-radius: 10px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                            max-width: 500px;
                            margin: 0 auto;
                        }}
                        .checkmark {{ 
                            color: #FF6B35; 
                            font-size: 60px; 
                        }}
                        h1 {{ color: #333; }}
                        .amount {{ 
                            font-size: 32px; 
                            color: #FF6B35; 
                            font-weight: bold;
                            margin: 20px 0;
                        }}
                        .reference {{
                            background: #f5f5f5;
                            padding: 10px;
                            border-radius: 5px;
                            margin: 20px 0;
                            font-family: monospace;
                        }}
                        .provider {{
                            color: #FF6B35;
                            font-weight: bold;
                        }}
                    </style>
                </head>
                <body>
                    <div class='success-box'>
                        <div class='checkmark'>✓</div>
                        <h1>Payment Successful!</h1>
                        <div class='amount'>₦{result.Amount:N2}</div>
                        <p>Your payment has been processed successfully via <span class='provider'>Flutterwave</span>.</p>
                        <div class='reference'>
                            Reference: {tx_ref}
                        </div>
                        <p style='color: #666; margin-top: 30px;'>
                            You can close this window now.
                        </p>
                    </div>
                </body>
                </html>
            ", "text/html");
                }
                else
                {
                    // Payment failed - show error
                    return Content($@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Payment Failed</title>
                    <style>
                        body {{ 
                            font-family: Arial, sans-serif; 
                            text-align: center; 
                            padding: 50px;
                            background: #f0f0f0;
                        }}
                        .error-box {{
                            background: white;
                            padding: 40px;
                            border-radius: 10px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                            max-width: 500px;
                            margin: 0 auto;
                        }}
                        .error-icon {{ 
                            color: #f44336; 
                            font-size: 60px; 
                        }}
                        h1 {{ color: #333; }}
                    </style>
                </head>
                <body>
                    <div class='error-box'>
                        <div class='error-icon'>✕</div>
                        <h1>Payment Failed</h1>
                        <p>{result.Message}</p>
                        <p style='color: #666; margin-top: 30px;'>
                            Please try again or contact support.
                        </p>
                    </div>
                </body>
                </html>
            ", "text/html");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Flutterwave callback");
                return StatusCode(500, "An error occurred processing your payment");
            }
        }

        [HttpPost("test-flutterwave")]
        public async Task<ActionResult<PaymentResult>> TestFlutterwave([FromBody] PaymentRequest request)
        {
            try
            {
                _logger.LogInformation("=== TEST FLUTTERWAVE STARTED ===");
                _logger.LogInformation($"Request: Amount={request.Amount}, Email={request.Email}");


                //generate reference
                var reference = $"FLW_{DateTime.UtcNow:yyyyMMddHHmmss}";
                _logger.LogInformation($"Generated reference: {reference}");

                //Get Flutterwave service
                var flutterwaveService = HttpContext.RequestServices.GetRequiredService<FlutterwaveService>();

                // initialize payment
                var result = await flutterwaveService.InitializePayment(request, reference);

                _logger.LogInformation("=== TEST FLUTTERWAVE COMPLETE ===");
                _logger.LogInformation($"Success: {result.Success}, URL: {result.AuthorizationUrl
                    }");

                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error in TestFlutterwave");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("payment-history")]
        public async Task<IActionResult> GetPaymentHistory(
            [FromQuery] string email = null,
            [FromQuery] string status = null,
            [FromQuery] int pageSize = 50
            ) 
        {
            try
            {
                var paymentRepo = HttpContext.RequestServices.GetRequiredService<IPaymentRepository>();

                var payments = await paymentRepo.GetPaymentsAsync(email, status, pageSize);

                _logger.LogInformation($"Retrieved {payments.Count} payments");

                return Ok(new
                {
                    count = payments.Count,
                    payments = payments.Select(p => new
                    {
                        p.PaymentId,
                        p.Reference,
                        p.Amount,
                        p.Currency,
                        p.Email,
                        p.Status,
                        p.PaymentMethod,
                        p.Provider,
                        p.PaidAt,
                        p.CreatedAt,
                        p.OrderId
                    })
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error getting payment history");
                return StatusCode(500, "Failed to get payments");
            }
        }

        [HttpGet("reference")]
        public async Task<IActionResult> GetPayment(string reference)
        {
            try
            {
                var paymentRepo = HttpContext.RequestServices.GetRequiredService<IPaymentRepository>();

                var payment = await paymentRepo.GetVerifiedPaymentByReferenceAsync(reference);

                if (payment == null)
                {
                    return NotFound(new { message = $"Payment {reference} not found" });
                }

                return Ok(payment);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error getting payment {reference}");
                return StatusCode(500, "Failed to get payment");
            }
        }

        [HttpGet("bank-details")]
        public async Task<IActionResult> GetBankDetails(
            [FromQuery] decimal amount,
            [FromQuery] string currency = "NGN")
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