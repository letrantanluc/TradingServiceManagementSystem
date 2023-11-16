using BaseProject.Controllers;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Service.Payment;
using Do_An_Chuyen_Nganh.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Infrastructure;
using System.Security.Cryptography;
using System.Web;
using System.Net;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class PayController : BaseController<Payment>
    {
        // GET: Pay
        private readonly CartManager _cartManager;
        private readonly MomoSettings _momoSettings; // Inject MomoResult
        private readonly VNPaySettings _vnPaySettings; // Inject MomoResult



        public PayController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, MomoSettings momoSettings, VNPaySettings vnPaySettings)
            : base(context)
        {
            _cartManager = new CartManager(httpContextAccessor);
            _momoSettings = momoSettings;
            _vnPaySettings = vnPaySettings;
        }

        public ActionResult ErrorPayment()
        {
            return View();
        }

        public ActionResult MomoPay()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("orderCode")))
            {
                return RedirectToAction("CheckOut", "Order");
            }

            var order = HttpContext.Session.GetObject<Order>("order");
            if (order == null)
            {
                return RedirectToAction("CheckOut", "Order");
            }

            string endPoint = _momoSettings.EndPoint;
            string partnerCode = _momoSettings.PartnerCode;
            string accessKey = _momoSettings.AccessKey;
            string secretKey = _momoSettings.SecretKey;
            string redirectUrl = _momoSettings.RedirectUrl;
            string ipnUrl = _momoSettings.IpnUrl;
            string requestType = "captureWallet";
            string orderInfo = "Thanh toan BaseStore #" + order.Code + "";
            string amount = string.Join("", order.Total.ToString("N0").Where(char.IsDigit)); // Xóa dấu phẩy
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType;

            MomoSecurity crypto = new MomoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, secretKey);

            //build body json request
            JObject message = new JObject
        {
            { "partnerCode", partnerCode },
            { "partnerName", "Test" },
            { "storeId", "MomoTestStore" },
            { "requestId", requestId },
            { "amount", amount },
            { "orderId", orderId },
            { "orderInfo", orderInfo },
            { "redirectUrl", redirectUrl },
            { "ipnUrl", ipnUrl },
            { "lang", "en" },
            { "extraData", extraData },
            { "requestType", requestType },
            { "signature", signature }
        };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endPoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult MomoProcessing(MomoSettings result)
        {
            if (result.resultCode == 0)
            {
               
                Order order = HttpContext.Session.GetObject<Order>("order");
                if (order != null)
                {
                    var cart = _cartManager.GetCartItems();
                    decimal totalOrder = 0;
                    foreach (var item in cart)
                    {
                        var itemTotal = item.Price;
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Order = order;
                        orderDetail.ProductId = item.ProductId;
                        orderDetail.ProductName = item.ProductName;
                        orderDetail.Price = item.Price;
                        orderDetail.Total = itemTotal;
                        orderDetail.Quantity = item.Quantity;
                        totalOrder += itemTotal;
                        _context.OrderDetails.Add(orderDetail);
                        _context.SaveChanges();
                    }
                    // Cập nhật tổng số tiền
                    order.Total = totalOrder;
                    // Cập nhật lại trạng thái đã thanh toán
                    order.Paid = 1;
                    _context.SaveChanges();

                    _cartManager.ClearCart();
                }
                return RedirectToAction("CompleteOrder", "Order");
            }
            return RedirectToAction("ErrorPayment", "Pay");
        }

        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }


        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret, Dictionary<string, string> _requestData)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }
        public ActionResult VNPay()
        {
            var order = HttpContext.Session.GetObject<Order>("order");
           
            if (order == null)
            {
                // Lỗi không tìm thấy order
                return RedirectToAction("CheckOut", "Order");
            }
            string vnp_Url = _vnPaySettings.Url;
            string vnp_ReturnUrl = _vnPaySettings.ReturnUrl;
            string vnp_HashSecret = _vnPaySettings.HashSecret;
            string vnp_TmnCode = _vnPaySettings.TmnCode; // Mã website của merchant trên hệ thống của VNPAY
            var vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Locale", "vn" }, //en= English, vn=Tiếng Việt
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", Guid.NewGuid().ToString() }, // Mã tham chiếu của giao dịch đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                { "vnp_OrderInfo", "Thanh toan BaseStore" + order.Code + "" },
                { "vnp_OrderType", "topup" } //Mã danh mục hàng hóa.
            };
            string amount = string.Join("", order.Total.ToString("N0").Where(char.IsDigit)); // Xóa dấu phẩy
            amount = (Int32.Parse(amount) * 100).ToString();
            vnp_Params.Add("vnp_Amount", amount); // Số tiền thanh toán
            vnp_Params.Add("vnp_ReturnUrl", vnp_ReturnUrl);
            vnp_Params.Add("vnp_IpAddr", GetIpAddress());
            vnp_Params.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //vnp_Params.Add("vnp_BankCode", "NCB");
            vnp_Params = vnp_Params.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);
            String signData = string.Join("&",
            vnp_Params.Where(x => !string.IsNullOrEmpty(x.Value))
            .Select(k => k.Key + "=" + k.Value));
            string paymentUrl = CreateRequestUrl(vnp_Url, vnp_HashSecret, vnp_Params);
            return Redirect(paymentUrl);
        }
        public ActionResult VNPayProcessing(int vnp_ResponseCode)
        {
            //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
            if (vnp_ResponseCode == 00)
            {
                Order order = HttpContext.Session.GetObject<Order>("order");
                if (order != null)
                {
                    var cart = _cartManager.GetCartItems();
                    decimal totalOrder = 0;
                    foreach (var item in cart)
                    {
                        var itemTotal = item.Price;
                        
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Order = order;
                        orderDetail.ProductId = item.ProductId;
                        orderDetail.ProductName = item.ProductName;
                        orderDetail.Price = item.Price;
                        orderDetail.Total = itemTotal;
                        orderDetail.Quantity = item.Quantity;
                        totalOrder += itemTotal;
                        _context.OrderDetails.Add(orderDetail);
                        _context.SaveChanges();
                    }
                    //Cập nhật tổng số tiền
                    order.Total = totalOrder;
                    _context.Orders.Update(order);
                    _cartManager.ClearCart();
                    //SendEmail(Session["email"].ToString(), order.Name, order.Code);
                    //Session["email"] = null;
                    // Cập nhật lại status paid
                    order.Paid = 1;
                    _context.SaveChanges();
                }


                return RedirectToAction("CompleteOrder", "Order");
            }
            return RedirectToAction("ErrorPayment", "Pay");
        }


    }
}
