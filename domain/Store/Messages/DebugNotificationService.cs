﻿using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
        }

        public Task SendConfirmationCodeAsync(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);

            return Task.CompletedTask;
        }

        public void StartProcess(Order order)
        {
            //using(var client = new SmtpClient())
            //{
            //    var message = new MailMessage("from@at.my.domain", "to@at.my.domain");
            //    message.Subject = "Заказ №" + order.Id;

            //    var builder = new StringBuilder();
            //    //var s = "";
            //    foreach(var item in order.Items)
            //    {
            //        builder.Append("{0}, {1}", item.BookId, item.Count);
            //        //s += string.Format("{0}, {1}", item.BookId, item.Count);
            //        //s += $"{item.BookId}, {item.Count}";
            //        builder.AppendLine();
            //        //s += Environment.NewLine;
            //    }

            //    message.Body = builder.ToString();
            //    client.Send(message);
            //}

            Debug.WriteLine("Order ID {0}", order.Id);
            Debug.WriteLine("Delivery: {0}", (object)order.Delivery.Description);
            Debug.WriteLine("Payment: {0}", (object)order.Payment.Description);
        }

        public Task StartProcessAsync(Order order)
        {
            StartProcess(order);

            return Task.CompletedTask;
        }
    }
}
