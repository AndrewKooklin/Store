﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public interface IOrderRepository
    {
        Order Create();

        Order GetById(int orderId);

        void Update(Order order);
    }
}