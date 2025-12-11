using BookNest.Library;
using BookNest.Models;
using Microsoft.AspNetCore.Http;

namespace BookNest.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal static string CreatePaymentUrl(double amount, string? returnUrl)
        {
            throw new NotImplementedException();
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
#pragma warning restore CS8604 // Possible null reference argument.
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
#pragma warning restore CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
#pragma warning restore CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_OrderType", model.OrderType);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
#pragma warning restore CS8604 // Possible null reference argument.
            pay.AddRequestData("vnp_TxnRef", tick);

#pragma warning disable CS8604 // Possible null reference argument.
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
#pragma warning restore CS8604 // Possible null reference argument.

            return paymentUrl;
        }


        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
#pragma warning disable CS8604 // Possible null reference argument.
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
#pragma warning restore CS8604 // Possible null reference argument.

            return response;
        }


    }
}
