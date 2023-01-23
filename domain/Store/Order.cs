﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> items;

        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }

        public int TotalCount => items.Sum(item => item.Count);
        //or
        //public int TotalCount
        //{
        //    get { return items.Sum(item => item.Count); }
        //}

        public decimal TotalPrice => items.Sum(item => item.Price * item.Count);

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if(items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;

            this.items = new List<OrderItem>(items);
        }

        public OrderItem GetItem(int bookId)
        {
            int index = items.FindIndex(item => item.BookId == bookId);

            if(index == -1)
            {
                ThrowBookExeption("Book not found", bookId);
            }

            return items[index];
        }

        //public bool ContainsItem(int bookId)
        //{
        //    return items.Any(item => item.BookId == bookId);
        //}

        public void AddOrUpdateItem(Book book, int count)
        {
            if(book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var index = items.FindIndex(item => item.BookId == book.Id);

            if(index == -1)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items[index].Count += count;
            }
        }

        public void RemoveItem(int bookId)
        {
            int index = items.FindIndex(itemId => itemId.BookId == bookId);

            if (index == -1)
            {
                 ThrowBookExeption("Order does not contain specified item:", bookId);
            }

            items.RemoveAt(index);

        }

        private void ThrowBookExeption(string message, int bookId)
        {
            var exeption = new InvalidOperationException(message);

            exeption.Data["BookId"] = bookId;

            throw exeption;
        }
    }
}