using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        public OrderItemCollection Items { get; }

        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }

        public int TotalCount => Items.Sum(item => item.Count);
        //or
        //public int TotalCount
        //{
        //    get { return items.Sum(item => item.Count); }
        //}

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count) 
                                     + (Delivery?.Amount ?? 0m);

        public Order(int id, IEnumerable<OrderItem> items)
        {
            Id = id;

            Items = new OrderItemCollection(items);
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
