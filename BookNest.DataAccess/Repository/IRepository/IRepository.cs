using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookNest.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hiển thị trang checkout
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePayment(OrderModel order)
        {
            if (order == null) return BadRequest();

            // Lưu đơn hàng vào database nếu cần (Pending)
            order.OrderId = Guid.NewGuid().ToString();
            order.Status = "Pending";
            // TODO: Save order vào DB

            // Cấu hình VNPay
            string? vnpUrl = _configuration["Vnpay:BaseUrl"];
            string? vnpReturnUrl = _configuration["Vnpay:PaymentBackReturnUrl"];
            string? vnpTmnCode = _configuration["Vnpay:TmnCode"];
            string? vnpHashSecret = _configuration["Vnpay:HashSecret"];

            var vnpParams = new Dictionary<string, string?>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnpTmnCode },
                { "vnp_Amount", (order.Amount * 100).ToString() }, // VNPay nhận amount * 100
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", order.OrderId },
                { "vnp_OrderInfo", $"Thanh toán đơn hàng {order.OrderId}" },
                { "vnp_OrderType", "other" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", vnpReturnUrl },
                { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
                { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") }
            };

            // Tạo chuỗi query
            var sortedParams = vnpParams.Where(kv => !string.IsNullOrEmpty(kv.Value)).OrderBy(x => x.Key).ToList();
            var query = new StringBuilder();
            var hashData = new StringBuilder();
            foreach (var param in sortedParams)
            {
                var encoded = WebUtility.UrlEncode(param.Value);
                query.Append($"{param.Key}={encoded}&");
                hashData.Append($"{param.Key}={param.Value}&");
            }
            string hashInput = hashData.ToString().TrimEnd('&');

            // ensure secret non-null
            if (string.IsNullOrEmpty(vnpHashSecret) || string.IsNullOrEmpty(vnpUrl))
            {
                return BadRequest("VNPay configuration missing");
            }

            string vnpSecureHash = CreateVnpaySha256(vnpHashSecret, hashInput);
            query.Append("vnp_SecureHash=" + vnpSecureHash);

            string paymentUrl = vnpUrl + "?" + query.ToString().TrimEnd('&');
            return Redirect(paymentUrl);
        }

        // Callback VNPay
        [HttpGet]
        public IActionResult VnpayReturn()
        {
            var queryParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            queryParams.TryGetValue("vnp_SecureHash", out var vnpSecureHash);
            queryParams.Remove("vnp_SecureHash");
            queryParams.Remove("vnp_SecureHashType");

            string vnpHashSecret = _configuration["Vnpay:HashSecret"] ?? string.Empty;
            var sortedParams = queryParams.OrderBy(x => x.Key);
            var hashData = new StringBuilder();
            foreach (var param in sortedParams)
            {
                hashData.Append($"{param.Key}={param.Value}&");
            }
            string hashInput = hashData.ToString().TrimEnd('&');
            string checkHash = CreateVnpaySha256(vnpHashSecret, hashInput);

            if (string.Equals(checkHash, vnpSecureHash, StringComparison.OrdinalIgnoreCase))
            {
                queryParams.TryGetValue("vnp_TxnRef", out var orderId);
                queryParams.TryGetValue("vnp_ResponseCode", out var responseCode);

                if (responseCode == "00")
                {
                    ViewBag.Message = "Thanh toán thành công!";
                }
                else
                {
                    ViewBag.Message = "Thanh toán thất bại hoặc bị hủy!";
                }
            }
            else
            {
                ViewBag.Message = "Sai chữ ký bảo mật!";
            }

            return View();
        }

        // Create HMAC-SHA512 hash (VNPay expects SHA512 in this code)
        private string CreateVnpaySha256(string secretKey, string data)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }
    }

    public class OrderModel
    {
        public decimal Amount { get; set; }
        public string? OrderId { get; set; }
        public string? Status { get; set; }
    }
}