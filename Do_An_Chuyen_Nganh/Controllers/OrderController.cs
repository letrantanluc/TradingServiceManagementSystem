using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Infrastructure;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class OrderController : BaseController<Order>
    {
        private readonly CartManager _cartManager;

        public OrderController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _cartManager = new CartManager(httpContextAccessor);
        }

        public static bool ValidateVNPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("+84", "0");
            Regex regex = new Regex(@"^(0)(86|96|97|98|32|33|34|35|36|37|38|39|91|94|83|84|85|81|82|90|93|70|79|77|76|78|92|56|58|99|59|55|87)\d{7}$");
            return regex.IsMatch(phoneNumber);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray()).ToUpper();
        }
        // GET: Order
        public ActionResult CheckOut()
        {
            var cart = _cartManager.GetCartItems();
            //if (!User.Identity.IsAuthenticated)
            //{
            //    // Người dùng chưa đăng nhập, chuyển hướng về trang đăng nhập
            //    return RedirectToAction("Login", "Account");
            //}
            if (cart.Count() < 1)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Carts = cart;
            return View(new Order());
        }
        [HttpPost]
        //public ActionResult CheckOut(Order Order)
        //{
        //    List<string> errors = new List<string>();
        //    try
        //    {
        //        var CustomerName = Order.CustomerName;
        //        var PhoneNumber = Order.PhoneNumber;
        //        var Address = Order.Address;
        //        var Payment = Order.Payment;
        //        var Email = Order.Email;


        //        if (string.IsNullOrEmpty(CustomerName))
        //        {
        //            errors.Add("Vui lòng nhập tên.");
        //        }

        //        if (string.IsNullOrEmpty(Address))
        //        {
        //            errors.Add("Vui lòng nhập địa chỉ.");
        //        }

        //        if (PhoneNumber != null && !ValidateVNPhoneNumber(PhoneNumber))
        //        {
        //            errors.Add("Số điện thoại không hợp lệ.");
        //        }

        //        switch (Payment)
        //        {
        //            case "cash":
        //            case "momo":
        //                break;
        //            default:
        //                errors.Add("Phương thức thanh toán không hợp lệ.");
        //                break;
        //        }

        //        if (errors.Count == 0)
        //        {
        //            Order order = new Order();

        //            string code = RandomString(12);
        //            order.Code = code;
        //            order.CustomerName = CustomerName;
        //            order.PhoneNumber = PhoneNumber;
        //            order.Address = Address;
        //            order.Payment = Payment;
        //            order.Email = Email;



        //            HttpContext.Session.SetString("orderCode", code);

        //            var cart = _cartManager.GetCartItems();
        //            decimal totalOrder = 0;
        //            foreach (var item in cart)
        //            {
        //                var itemTotal = item.Price * item.Quantity;
        //                totalOrder += itemTotal;
        //            }
        //            order.Total = totalOrder;
        //            HttpContext.Session.SetObject("order", order);
        //            totalOrder = 0;


        //            foreach (var item in cart)
        //            {
        //                var itemTotal = item.Price * item.Quantity;
        //                foreach (var option in cart)
        //                {
        //                    itemTotal += option.Price * item.Quantity;
        //                }
        //                OrderDetail orderDetail = new OrderDetail();
        //                orderDetail.Order = order;
        //                orderDetail.ProductId = item.ProductId;

        //                orderDetail.Price = item.Price;
        //                orderDetail.Total = itemTotal;
        //                orderDetail.Quantity = item.Quantity;
        //                totalOrder += itemTotal;
        //                _context.OrderDetails.Add(orderDetail);
        //                _context.SaveChanges();
        //            }
        //            //Cập nhật tổng số tiền
        //            order.Total = totalOrder;
        //            Update(order);
        //            _cartManager.ClearCart();



        //            return RedirectToAction("CompleteOrder", "Order");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errors.Add(ex.Message);
        //    }
        //    TempData["Errors"] = errors;
        //    return RedirectToAction("CheckOut", "Order");
        //}

        public ActionResult CheckOut(Order Order)
        {
            List<string> errors = new List<string>();
            try
            {
                var CustomerName = Order.CustomerName;
                var PhoneNumber = Order.PhoneNumber;
                var Address = Order.Address;
                var Payment = Order.Payment;
                var Email = Order.Email;


                if (string.IsNullOrEmpty(CustomerName))
                {
                    errors.Add("Vui lòng nhập tên.");
                }

                if (string.IsNullOrEmpty(Address))
                {
                    errors.Add("Vui lòng nhập địa chỉ.");
                }

                if (PhoneNumber != null && !ValidateVNPhoneNumber(PhoneNumber))
                {
                    errors.Add("Số điện thoại không hợp lệ.");
                }

                switch (Payment)
                {
                    case "cash":
                    case "momo":
                    case "vnpay":

                        break;
                    default:
                        errors.Add("Phương thức thanh toán không hợp lệ.");
                        break;
                }

                if (errors.Count == 0)
                {
                    Order order = new Order();

                    string code = RandomString(12);
                    order.Code = code;
                    order.CustomerName = CustomerName;
                    order.PhoneNumber = PhoneNumber;
                    order.Address = Address;
                    order.Payment = Payment;
                    order.Email = Email;



                    HttpContext.Session.SetString("orderCode", code);

                    var cart = _cartManager.GetCartItems();
                    decimal totalOrder = 0;
                    foreach (var item in cart)
                    {
                        var itemTotal = item.Price * item.Quantity;
                        totalOrder += itemTotal;
                    }
                    order.Total = totalOrder;
                    HttpContext.Session.SetObject("order", order);
                    switch (Payment)
                    {
                        case "momo":
                            return RedirectToAction("MomoPay", "Pay");
                        case "vnpay":
                            return RedirectToAction("VNPay", "Pay");
                        default:
                            totalOrder = 0;
                            foreach (var item in cart)
                            {
                                var itemTotal = item.Price * item.Quantity;
                                foreach (var option in cart)
                                {
                                    itemTotal += option.Price * item.Quantity;
                                }
                                OrderDetail orderDetail = new OrderDetail();
                                orderDetail.Order = order;
                                orderDetail.ProductId = item.ProductId;

                                orderDetail.Price = item.Price;
                                orderDetail.Total = itemTotal;
                                orderDetail.Quantity = item.Quantity;
                                totalOrder += itemTotal;
                                _context.OrderDetails.Add(orderDetail);
                                _context.SaveChanges();
                            }
                            //Cập nhật tổng số tiền
                            order.Total = totalOrder;
                            Update(order);
                            _cartManager.ClearCart();
                            break;
                    }
                    return RedirectToAction("CompleteOrder", "Order");
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("CheckOut", "Order");
        }

        public ActionResult CompleteOrder()
        {
            
            return View();
        }
        public ActionResult CheckOutError()
        {
            // Thông báo lỗi cho người dùng hoặc thực hiện các xử lý khác
            return View();
        }
        public ActionResult DangTin()
        {
            return View();
        }
    }
}
