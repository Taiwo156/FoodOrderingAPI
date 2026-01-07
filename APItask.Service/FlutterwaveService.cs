using APItask.Core.Models;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class FlutterwaveService : IPaymentProviderService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FlutterwaveService> _logger;
        private readonly RestClient _client;
        private readonly string _publicKey;
        private readonly string _secretKey;
        private readonly string _encryptionKey;

        public FlutterwaveService(IConfiguration configuration, ILogger<FlutterwaveService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // get configuration vakues
            var baseUrl = _configuration["PaymentProviders:Flutterwave:BaseUrl"];
            _secretKey = _configuration["PaymentProviders:Flutterwave:SecretKey"];
            _publicKey = _configuration["PaymentProviders:Flutterwave:PublicKey"];
            _encryptionKey = _configuration["PaymentProviders:Flutterwave:EncryptionKey"];

            // Initialize rest client
            _client = new RestClient(baseUrl);

            _logger.LogInformation($"FlutterwaveService initialized with base URL : {baseUrl}");
        }

        public async Task<PaymentResult> InitializePayment(PaymentRequest request, string reference)
        {
            try
            {
                _logger.LogInformation($"Flutterwave: Initializing payment - Reference: {reference} , Amount: {request.Amount}");

                // get callback url from configuration
                var callbackUrl = _configuration["PaymentProviders:Flutterwave:CallbackUrl"];

                // request payment request payload
                var payload = new
                {
                    tx_ref = reference,
                    amount = request.Amount.ToString(),
                    currency = request.Currency ?? "NGN",
                    redirect_url = callbackUrl,
                    customer = new
                    {
                        email = request.Email,
                        phone = request.Phone,
                        name = request.Email.Split("@")[0]
                    },
                    customizations = new
                    {
                        title = "Food Order Payment",
                        description = $"Payment for order {reference}",
                        logo = " " //your logo
                    }
                };

                _logger.LogInformation("Sending request to flutterwave...");

                // create rest request
                var restRequest = new RestRequest("/payments", Method.Post);
                restRequest.AddHeader("Authorization", $"Bearer {_secretKey}");
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddJsonBody(payload);

                // execute  request
                var response = await _client.ExecuteAsync(restRequest);

                _logger.LogInformation($"Flutterwave response: {response.Content}");

                if (response.IsSuccessful)
                {
                    // parse response
                    dynamic responseData = JsonConvert.DeserializeObject(response.Content);

                    if (responseData.status == "success")
                    {
                        string authorizationUrl = responseData.data.link;

                        _logger.LogInformation($"Payment initialized successfully! URL: {authorizationUrl}");

                        return new PaymentResult
                        {
                            Success = true,
                            Message = "Redirect customer to Flutterwave to complete payment",
                            Reference = reference,
                            AuthorizationUrl = authorizationUrl,
                            Amount = request.Amount
                        };
                    }
                }
                _logger.LogError($"Payment Initialization failed: {response.Content}");

                return new PaymentResult
                {
                    Success = false,
                    Message = "Failed to initialize payment with Flutterwave",
                    Reference = reference,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Flutterwave InitializePayment");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Reference = reference
                };
            }
        }
        public async Task<PaymentResult> VerifyPayment(string reference)
        {
            try
            {
                _logger.LogInformation($"Flutterwave: Verifying Payment - Reference {reference}");

                // create REST request
                var restRequest = new RestRequest($"/transactions/verify_by_reference?tx_ref={reference}", Method.Get);
                restRequest.AddHeader("Authorization", $"Bearer {_secretKey}");

                // execute request
                var response = await _client.ExecuteAsync(restRequest);

                _logger.LogInformation($"Flutterwave verify response: {response.Content}");

                if (response.IsSuccessful)
                {
                    dynamic responseData = JsonConvert.DeserializeObject(response.Content);

                    if (responseData.status == "success")
                    {
                        var transactionData = responseData.data;
                        string status = transactionData.status;
                        decimal amount = transactionData.amount;


                        _logger.LogInformation($"Payment verification complete: Status{status}, Amount{amount}");

                        if (status == "successful") {
                            return new PaymentResult
                            {
                                Success = true,
                                Message = "Payment verified successfully!",
                                Reference = reference,
                                Amount = amount
                            };
                        }
                        else
                        {
                            return new PaymentResult
                            {
                                Success = false,
                                Message = $"Payment status: {status}",
                                Reference = reference,
                            };
                        }
                    }
                }
                _logger.LogInformation($"Payment verification failed: {response.Content}");

                return new PaymentResult
                {
                    Success = false,
                    Message = "Payment verification failed",
                    Reference = reference
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Fluterwave VerifyPayment");

                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Reference = reference
                };
            }
        }
        public string GetProviderName()
        {
            return "Flutterwave";
        }
    }
}
