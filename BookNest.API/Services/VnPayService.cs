//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using BookNest.Library;
//using BookNest.Models;
//using Microsoft.AspNetCore.Http;

//namespace BookNest.Services
//{
//    public class VnPayService : IVnPayService
//    {
//        private readonly IConfiguration _configuration;

//        public VnPayService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        internal static string CreatePaymentUrl(double amount, string? returnUrl)
//        {
//            throw new NotImplementedException();
//        }

//        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
//        {
//#pragma warning disable CS8604 // Possible null reference argument.
//            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
//            var tick = DateTime.Now.Ticks.ToString();
//            var pay = new VnPayLibrary();
//            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning restore CS8604 // Possible null reference argument            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
//            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
//#pragma warning restore CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_OrderType", model.OrderType);
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning disable CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
//#pragma warning restore CS8604 // Possible null reference argument.
//            pay.AddRequestData("vnp_TxnRef", tick);

//#pragma warning disable CS8604 // Possible null reference argument.
//            var paymentUrl =
//                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
//#pragma warning restore CS8604 // Possible null reference argument.

//            return paymentUrl;
//        }


//        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
//        {
//            var pay = new VnPayLibrary();
//#pragma warning disable CS8604 // Possible null reference argument.
//            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
//#pragma warning restore CS8604 // Possible null reference argument.

//            return response;
//        }

//		public string CreateVnPayPayment(Order order)
//		{
//			var config = _configuration.GetSection("Vnpay");

//			string tmnCode = config["TmnCode"];
//			string hashSecret = config["HashSecret"];
//			string baseUrl = config["BaseUrl"];
//			string returnUrl = config["PaymentBackReturnUrl"];

//			var tick = DateTime.Now.Ticks.ToString();

//			var vnp_Params = new SortedDictionary<string, string>
//	{
//		{ "vnp_Version", "2.1.0" },
//		{ "vnp_Command", "pay" },
//		{ "vnp_TmnCode", tmnCode },
//		{ "vnp_Amount", ((long)(order.TotalAmount * 100)).ToString() },
//		{ "vnp_BankCode", "" },
//		{ "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
//		{ "vnp_CurrCode", "VND" },
//		{ "vnp_IpAddr", "127.0.0.1" },
//		{ "vnp_Locale", "vn" },
//		{ "vnp_OrderInfo", "Payment for Order " + order.Id },
//		{ "vnp_OrderType", "other" },
//		{ "vnp_ReturnUrl", returnUrl + "?orderId=" + order.Id },
//		{ "vnp_TxnRef", tick }
//	};

//			string query = string.Join("&", vnp_Params.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"));

//			string secureHash = HmacSHA512(hashSecret, query);

//			return $"{baseUrl}?{query}&vnp_SecureHash={secureHash}";
//		}

//		private string HmacSHA512(string key, string input)
//		{
//			using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
//			byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
//			return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
//		}
//	}
//}
