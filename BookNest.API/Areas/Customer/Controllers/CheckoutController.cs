 using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;
namespace YourProject.Controllers
    {
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
            public IActionResult CreatePayment(OrderModel order) // OrderModel chứa thông tin đơn hàng
            {
                // Lưu đơn hàng vào database nếu cần (Pending)
                order.OrderId = Guid.NewGuid().ToString();
                order.Status = "Pending";
                // TODO: Save order vào DB

                // Cấu hình VNPay
                string? vnpUrl = _configuration["VnPay:Url"];
                string? vnpReturnUrl = _configuration["VnPay:ReturnUrl"];
                string? vnpTmnCode = _configuration["VnPay:TmnCode"];
                string? vnpHashSecret = _configuration["VnPay:HashSecret"];

                var vnpParams = new Dictionary<string,string?>
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
            { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress.ToString() },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
        };

                // Tạo chuỗi query
                var sortedParams = vnpParams.OrderBy(x => x.Key).ToList();
                var query = new StringBuilder();
                var hashData = new StringBuilder();
                foreach (var param in sortedParams)
                {
                    query.Append($"{param.Key}={HttpUtility.UrlEncode(param.Value)}&");
                    hashData.Append($"{param.Key}={param.Value}&");
                }
                string hashInput = hashData.ToString().TrimEnd('&');
                string vnpSecureHash = CreateVnpaySha256(vnpHashSecret, hashInput);
                query.Append("vnp_SecureHash=" + vnpSecureHash);

                string paymentUrl = vnpUrl + "?" + query.ToString();
                return Redirect(paymentUrl);
            }

            // Callback VNPay
            public IActionResult VnpayReturn()
            {
                var queryParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

                string vnpSecureHash = queryParams["vnp_SecureHash"];
                queryParams.Remove("vnp_SecureHash");
                queryParams.Remove("vnp_SecureHashType");
               string vnpHashSecret = _configuration["VnPay:HashSecret"];
                var sortedParams = queryParams.OrderBy(x => x.Key);
                var hashData = new StringBuilder();
                foreach (var param in sortedParams)
                {
                    hashData.Append($"{param.Key}={param.Value}&");
                }
                string hashInput = hashData.ToString().TrimEnd('&');
                string checkHash = CreateVnpaySha256(vnpHashSecret, hashInput);

                if (checkHash == vnpSecureHash)
                {
                    string orderId = queryParams["vnp_TxnRef"];
                    string responseCode = queryParams["vnp_ResponseCode"];

                    // TODO: Cập nhật trạng thái đơn hàng
                    if (responseCode == "00")
                    {
                        // Thanh toán thành công
                        // Cập nhật order.Status = "Paid"
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

            // Hàm tạo hash SHA256
            private string CreateVnpaySha256(string secretKey, string data)
            {
                var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }
public class OrderModel
    {
        internal int Amount;

        public string OrderId { get; internal set; }
        public string Status { get; internal set; }
    }
}
            