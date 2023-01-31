using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Order
    {
        private readonly OrderDto dto;

        public int Id => dto.Id;

        public OrderItemCollection Items { get; }

        public string CellPhone 
        {
            get => dto.CellPhone;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(CellPhone));
                }

                dto.CellPhone = value;
            } 
        }

        public OrderDelivery Delivery 
        { get 
            { 
                if(dto.DeliveryUniqueCode == null)
                {
                    return null;
                }

                return new OrderDelivery(
                        dto.DeliveryUniqueCode,
                        dto.DeliveryDescription,
                        dto.DeliveryPrice,
                        dto.DeliveryParameters);
            } 
            set
            {
                if(value == null)
                {
                    throw new ArgumentException(nameof(Delivery));
                }

                dto.DeliveryUniqueCode = value.UnuqueCode;
                dto.DeliveryDescription = value.Description;
                dto.DeliveryPrice = value.Price;
                dto.DeliveryParameters = value.Parameters
                                              .ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public OrderPayment Payment 
        { get 
            {
                if(dto.PaymentServiceName == null)
                {
                    return null;
                }

                return new OrderPayment(
                    dto.PaymentServiceName,
                    dto.PaymentDescription,
                    dto.PaymentParameters);
            }
          set
            {
                if(value == null)
                {
                    throw new ArgumentException(nameof(Payment));
                }

                dto.PaymentServiceName = value.UniqueCode;
                dto.PaymentDescription = value.Description;
                dto.PaymentParameters = value.Parameters
                                             .ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public int TotalCount => Items.Sum(item => item.Count);
        //or
        //public int TotalCount
        //{
        //    get { return items.Sum(item => item.Count); }
        //}

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count) 
                                     + (Delivery?.Price ?? 0m);

        public Order(OrderDto dto)
        {
            this.dto = dto;

            Items = new OrderItemCollection(dto);
        }

        public static class DtoFactory
        {
            public static OrderDto Create() => new OrderDto();
        }

        public static class Mapper
        {
            public static Order Map(OrderDto dto) => new Order(dto);

            public static OrderDto Map(Order domain) => domain.dto;
        }

        //public OrderItem GetItem(int bookId)
        //{
        //    int index = items.FindIndex(item => item.BookId == bookId);

        //    if(index == -1)
        //    {
        //        ThrowBookExeption("Book not found", bookId);
        //    }

        //    return items[index];
        //}

        //public bool ContainsItem(int bookId)
        //{
        //    return items.Any(item => item.BookId == bookId);
        //}

        //public void AddOrUpdateItem(Book book, int count)
        //{
        //    if(book == null)
        //    {
        //        throw new ArgumentNullException(nameof(book));
        //    }

        //    var index = items.FindIndex(item => item.BookId == book.Id);

        //    if(index == -1)
        //    {
        //        items.Add(new OrderItem(book.Id, book.Price, count));
        //    }
        //    else
        //    {
        //        items[index].Count += count;
        //    }
        //}

        //public void RemoveItem(int bookId)
        //{
        //    int index = items.FindIndex(itemId => itemId.BookId == bookId);

        //    if (index == -1)
        //    {
        //         ThrowBookExeption("Order does not contain specified item:", bookId);
        //    }

        //    items.RemoveAt(index);

        //}

        private void ThrowBookExeption(string message, int bookId)
        {
            var exeption = new InvalidOperationException(message);

            exeption.Data["BookId"] = bookId;

            throw exeption;
        }
    }
}
