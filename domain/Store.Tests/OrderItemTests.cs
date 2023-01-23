using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOfRangeExeption()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            {
                int count = 0;
                new OrderItem(1, count, 0m);
            });
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOfRangeExeption()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, count, 0m);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetCount()
        {
            var orderItem = new OrderItem(1, 2, 3m);

            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, count, 0m);
            });
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItem = new OrderItem(0, 5, 0m);

          Assert.Throws<ArgumentOutOfRangeException>(() => 
          {
              orderItem.Count = -1;

          });
        }

        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItem = new OrderItem(0, 5, 0m);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });
        }

        [Fact]
        public void Count_WithPositiveValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItem = new OrderItem(0, 5, 0m);

            orderItem.Count = 10;

            Assert.Equal(10, orderItem.Count);
        }

        
    }


}
