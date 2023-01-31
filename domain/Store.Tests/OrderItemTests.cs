using Store.Data;
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
                OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, count);
            });
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOfRangeExeption()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, count);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetCount()
        {
            var orderItem = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 2);

            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(10m, orderItem.Price);
            Assert.Equal(2, orderItem.Count);
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            Assert.Throws<ArgumentOutOfRangeException>(() => 
          {
              orderItem.Count = -1;

          });
        }

        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });
        }

        [Fact]
        public void Count_WithPositiveValue_ThrowsArgumentOfRangeExeption()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            orderItem.Count = 10;

            Assert.Equal(10, orderItem.Count);
        }
    }
}
