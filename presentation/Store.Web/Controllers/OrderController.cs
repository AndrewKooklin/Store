using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using Store.Contractors;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Store.Web.Contractors;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        //private readonly OrderService orderService;
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        //private readonly INotificationService notification;

        public OrderController(//OrderService orderService,
                                IBookRepository bookRepository,
                               IOrderRepository orderRepository,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IPaymentService> paymentServices,
                               IEnumerable<IWebContractorService> webContractorServices
                               //INotificationService notificationService
                               )
        {
            //this.orderService = orderService;
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
            //this.notificationService = notificationService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);

                return View(model);
            }

            return View("Empty");
        }



        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count = item.Count,
                             };

            return new OrderModel {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }

        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var book = bookRepository.GetById(bookId);

            if(order.Items.TryGet(bookId, out OrderItem orderItem))
            {
                orderItem.Count += count;
            }
            else
            {
                order.Items.Add(bookId, book.Price, count);
            }

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.Items.Get(bookId).Count = count;

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.Items.Remove(bookId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;

            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id, 0, 0m);
            }

            return (order, cart);
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);

            HttpContext.Session.Set(cart);
        }

        //public IActionResult RemoveBook(int id)
        //{
        //    (Order order, Cart cart) = GetOrCreateOrderAndCart();

        //    order.GetItem(id).Count--;

        //    SaveOrderAndCart(order, cart);

        //    return RedirectToAction("Index", "Book", new { id });
        //}

        

        //[HttpPost]
        //public IActionResult SendConfirmationCode(int id, string cellPhone)
        //{
        //    var order = orderRepository.GetById(id);
        //    var model = Map(order);

        //    if (!IsValidCellPhone(cellPhone))
        //    {
        //        model.Errors["cellPhone"] = "Номер телефона не соответствует формату";
        //        return View("Index", model);
        //    }

        //    int code = 1111;
        //    HttpContext.Session.SetInt32(cellPhone, code);
        //    notificationService.SendConfirmationCode(cellPhone, code);

        //    return View("Confirmation",
        //        new ConfirmationModel
        //        {
        //            OrderId = id,
        //            CellPhone = cellPhone
        //        });
        //}

        private bool IsValidCellPhone(string cellPhone)
        {
            if(cellPhone == null)
            {
                return false;
            }

            cellPhone = cellPhone.Replace(" ", "").Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        [HttpPost]
        public IActionResult Confirmate(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if(storedCode == null)
            {
                return View("Confirmation",
                    new ConfirmationModel
                    {
                        OrderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string>
                        {
                            {"code", "Пустой код, повторите отправку" }
                        },
                    });
            }
            if(storedCode != code)
            {
                return View("Confirmation",
                    new ConfirmationModel
                    {
                        OrderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string>
                        {
                            {"code", "Код отличается от отправленного" }
                        },
                    });
            }

            var order = orderRepository.GetById(id);
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
                                                        service => service.Title)
            };

            return View("DeliveryMethod", model);
        }

        //[HttpPost]
        //public IActionResult ConfirmCellPhone(string cellPhone, int confirmationCode)
        //{
        //    var model = orderService.ConfirmCellPhoneAsync(cellPhone, confirmationCode);

        //    if (model.Errors.Count > 0)
        //        return View("Confirmation", model);

        //    var deliveryMethods = deliveryServices.ToDictionary(service => service.UniqueCode,
        //                                                        service => service.Title);

        //    return View("DeliveryMethod", deliveryMethods);
        //}

        //[HttpPost]
        //public IActionResult StartDelivery(int id, string cellPhone)
        //{
        //    var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
        //    var order = await orderService.GetOrderAsync();
        //    var form = deliveryService.FirstForm(order);

        //    var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
        //    if (webContractorService == null)
        //        return View("DeliveryStep", form);

        //    var returnUri = GetReturnUri(nameof(NextDelivery));
        //    var redirectUri = await webContractorService.StartSessionAsync(form.Parameters, returnUri);

        //    return Redirect(redirectUri.ToString());

        //}

        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);

            var order = orderRepository.GetById(id);

            var form = deliveryService.CreateForm(order);

            return View("DeiveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);

            var form = deliveryService.MoveNextForm(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliveryService.GetDelivery(form);
                orderRepository.Update(order);

                var model = new DeliveryModel
                {
                    OrderId = id,
                    Methods = paymentServices.ToDictionary(service => service.UniqueCode,
                                                           service => service.Title)
                };

                return View("PaymentMethod", model);
            }

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult StartPayment(int id, string uniqueCode)
        {
            var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);

            var order = orderRepository.GetById(id);

            var form = paymentService.CreateForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);

            if(webContractorService != null)
            {
                return Redirect(webContractorService.GetUri);
            }

            return View("PaymentStep", form);
        }

        [HttpPost]
        public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);

            var form = paymentService.MoveNextForm(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                orderRepository.Update(order);

                return View("Finish");
            }

            return View("PaymentStep", form);
        }

        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart();

            return View();
        }
    }
}
