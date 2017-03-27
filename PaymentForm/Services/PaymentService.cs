using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentForm.Domain;

namespace PaymentForm.Services
{
    public class PaymentService
    {
        private readonly PaymentSettings _paymentSettings;

        public PaymentService(PaymentSettings paymentSettings)
        {
            _paymentSettings = paymentSettings;
        }

        public async Task<PaymentResponse> ChargeAsync(ChargeRequest chargeRequest)
        {
            return await ProcessRequestAsync(async httpClient =>
            {
                httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await httpClient.PostAsJsonAsync("charge", chargeRequest);
            });
        }

        public async Task<PaymentResponse> PostThreeDSAsync(int transactionId, string paRes)
        {
            return await ProcessRequestAsync(async httpClient =>
            {
                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("TransactionId", transactionId.ToString()),
                    new KeyValuePair<string, string>("PaRes", paRes)
                };
                return await httpClient.PostAsync("post3ds", new FormUrlEncodedContent(keyValues));
            });
        }

        private async Task<PaymentResponse> ProcessRequestAsync(Func<HttpClient, Task<HttpResponseMessage>> processRequest)
        {
            using (var httpClient = new HttpClient())
            {
                var paymentResponse = new PaymentResponse();
                try
                {
                    httpClient.BaseAddress = new Uri("https://api.cloudpayments.ru/payments/cards/");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(
                                System.Text.Encoding.ASCII.GetBytes($"{_paymentSettings.PublicId}:{_paymentSettings.ApiKey}")));

                    var response = await processRequest(httpClient);

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic responseObject =
                            JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                        MapToPaymentResponse(responseObject, paymentResponse);
                    }
                }
                catch
                {
                    paymentResponse.IsSuccess = false;
                }

                return paymentResponse;
            }
        }

        private void MapToPaymentResponse(dynamic responseObject, PaymentResponse paymentResponse)
        {
            paymentResponse.IsSuccess = responseObject.Success;
            paymentResponse.Message = responseObject.Model?.CardHolderMessage;
            paymentResponse.TransactionId = responseObject.Model?.TransactionId ?? default(int);
            paymentResponse.OrderId = responseObject.Model?.InvoiceId ?? default(int);
            paymentResponse.Amount = responseObject.Model?.PaymentAmount ?? default(decimal);
            paymentResponse.Currency = responseObject.Model?.PaymentCurrency;
            paymentResponse.CreatedDate = responseObject.Model?.CreatedDate ?? default(DateTime);
            if (responseObject.Model?.PaReq != null && responseObject.Model?.AcsUrl != null)
            {
                paymentResponse.ThreeDSecure = new ThreeDSecure
                {
                    PaReq = responseObject.Model.PaReq,
                    AcsUrl = responseObject.Model.AcsUrl
                };
            }
        }
    }
}