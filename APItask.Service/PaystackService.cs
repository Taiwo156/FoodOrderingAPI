using Apitask.Core.DTOs.PaymentProviders;
using APItask.Core.Models;
using APItask.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class PaystackService : IPaymentProviderService
    {
        private readonly ILogger<PaystackService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _baseUrl;
        private readonly string _callbackUrl;



        public PaystackService(ILogger<PaystackService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            // Check if the configuration section exists
            var paystackConfig = _configuration.GetSection("PaymentProviders:Paystack");

            if (!paystackConfig.Exists())
            {
                _logger.LogError("Paystack configuration section not found!");
                // Try alternative path
                paystackConfig = _configuration.GetSection("Paystack");
            }

            _secretKey = paystackConfig["SecretKey"];
            _baseUrl = paystackConfig["BaseUrl"] ?? "https://api.paystack.co";
            _callbackUrl = paystackConfig["CallbackUrl"];

            if (string.IsNullOrEmpty(_secretKey))
            {
                _logger.LogCritical("Paystack SecretKey is null or empty!");
                throw new InvalidOperationException("Paystack secretkey not configured! Check appsettings.json");
            }

            if (!_secretKey.StartsWith("sk_"))
            {
                _logger.LogWarning("SecretKey doesn't start with 'sk_' - might be invalid format");
            }

            _logger.LogInformation("PaystackService Initialized with base URL: {BaseUrl}", _baseUrl);
            _logger.LogDebug("Callback URL: {CallbackUrl}", _callbackUrl);
        }
        public string GetProviderName() => "Paystack";
        public async Task<PaymentResult> InitializePayment(PaymentRequest request, string reference)
        {
            try
            {
                _logger.LogInformation("Paystack Initializing Payment : Reference = {Reference}, Amount = {Amount}, Email = {Email}", reference, request.Amount, request.Email);
                // FIX: Create RestSharp client properly
                var options = new RestClientOptions(_baseUrl)
                {
                    Timeout = TimeSpan.FromSeconds(30), // 30 seconds
                    ThrowOnAnyError = false
                };

                //STEP 1: Request RestSharp client
                var client = new RestClient(options);

                //STEP 2: Create POST request to /transaction/initialize
                var restRequest = new RestRequest("/transaction/initialize", Method.Post);

                //STEP 3 : Add authorization header
                restRequest.AddHeader("Authorization", $"Bearer {_secretKey}");
                restRequest.AddHeader("Content-Type", "application/json");

                //STEP 4: Convert amount to kobo(multiply by 100)
                var amountInKobo = (long)(request.Amount * 100);

                _logger.LogInformation("Converted ₦{Amount} to {Kobo} kobo", request.Amount, amountInKobo);

                //STEP 5: Prepare request body
                var paystackRequest = new PaystackInitializeRequest
                {
                    email = request.Email,
                    amount = amountInKobo,
                    reference = reference,
                    callback_url = _callbackUrl,
                    metadata = new Dictionary<string, object>
                    {
                        {"phone", request.Phone ?? "N/A" },
                        {"payment_method", request.Method }
                    }
                };

                //STEP 6: Add JSON body
                restRequest.AddJsonBody
                    (paystackRequest);
                _logger.LogInformation("Sending request to Paystack...");

                //STEP 7: Make the API call
                var response = await client.ExecuteAsync(restRequest);

                //STEP 8: Check HTTP status
                if (!response.IsSuccessful)
                {
                    _logger.LogError("Paystack API call failed: Status = {StatusCode}, Error = {LogError}, Content = {Content}",
                        response.StatusCode, response.ErrorMessage, response.Content);

                    return new PaymentResult
                    {
                        Success = false,
                        Message = $"Payment Initialization failed: {response.ErrorMessage}"
                    };
                }
                _logger.LogInformation("Paystack response received:{Content}", response.Content);

                //STEP 9 : Parse JSON response
                var paystackResponse = JsonConvert.DeserializeObject<PaystackInitializeResponse>(response.Content);

                //STEP 10: Check Paystack response status
                if (paystackResponse == null || !paystackResponse.status) {
                    var errorMsg = paystackResponse?.message ?? "Unknown error";

                    _logger.LogError("Full Paystack error response: {Response}", response.Content);

                    _logger.LogWarning("Paystack returned failure: {Message}", errorMsg);


                    return new PaymentResult
                    {
                        Success = false,
                        Message = errorMsg
                    };
                }

                //STEP 11: Success! Return authorization URL
                _logger.LogInformation(
                    "Paystack payment initialized successfully! URL: {Url}",
                    paystackResponse.data.authorization_url);

                return new PaymentResult
                {
                    Success = true,
                    Message = "Redirect customer to Paystack to complete payment",
                    Reference = reference,
                    AuthorizationUrl = paystackResponse.data.authorization_url
                };

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "❌ Error initializing Paystack payment");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResult> VerifyPayment(string reference)
        {
            try
            {
                _logger.LogInformation("Verifying Paystack payment: {Reference}", reference);

                var client = new RestClient(_baseUrl);
                var restRequest = new RestRequest($"/transaction/verify/{reference}", Method.Get);
                restRequest.AddHeader("Authorization", $"Bearer {_secretKey}");

                var response = await client.ExecuteAsync(restRequest);

                if (!response.IsSuccessful)
                {
                    _logger.LogError(
                        "Paystack verify failed: Status={StatusCode}, Content={Content}",
                        response.StatusCode, response.Content);

                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Failed to verify payment",
                        Reference = reference
                    };
                }

                var verifyResponse = JsonConvert.DeserializeObject<PaystackVerifyResponse>(response.Content);

                if (verifyResponse == null || !verifyResponse.status)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = verifyResponse?.message ?? "Verification failed",
                        Reference = reference
                    };
                }

                // Check actual payment status
                var paymentStatus = verifyResponse.data.status.ToLower();
                var isSuccessful = paymentStatus == "success";

                _logger.LogInformation(
                    "Payment verification complete: Status={Status}, Amount={Amount}",
                    paymentStatus, verifyResponse.data.amount / 100m);

                return new PaymentResult
                {
                    Success = isSuccessful,
                    Message = isSuccessful
                        ? "Payment verified successfully!"
                        : $"Payment {paymentStatus}",
                    Reference = reference,
                    Amount = verifyResponse.data.amount / 100m  // Convert back from kobo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error verifying Paystack payment");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Verification error: {ex.Message}",
                    Reference = reference
                };
            }
        }
    }
    
}
