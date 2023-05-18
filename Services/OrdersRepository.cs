﻿using Microsoft.EntityFrameworkCore;
using WebService.Model;

namespace WebService.Services;

public class OrdersRepository : IOrdersRepository
{
    private readonly NorthwindContext context;

    public OrdersRepository(NorthwindContext context)
    {
        this.context = context;
    }

    public IEnumerable<Order> GetAllOrders() => context.Orders.AsNoTracking().ToArray();

    public Order? GetOrderByID(int? id)
    {
        return context.Orders
            .Include(o => o.OrderDetails)
            .Include(nameof(Order.OrderDetails) + "." + nameof(OrderDetail.Product))
            .FirstOrDefault(o => o.OrderId == id);
    }

    public Order CreateNewOrder(Order newOrder)
    {
        context.Add(newOrder);
        context.SaveChanges();
        return newOrder;
    }

    public async Task<Order> UpdateOrder(int id, Order updatedOrder)
    {
        var existedOrder = GetOrderByID(id);
        if(existedOrder == null)
        {
            throw new ArgumentException("No order exists with the given id", nameof(id));
        }
        else
        {
            existedOrder.OrderDate = updatedOrder.OrderDate;
            existedOrder.RequiredDate = updatedOrder.RequiredDate;
            existedOrder.ShippedDate = updatedOrder.ShippedDate;
            existedOrder.Freight = updatedOrder.Freight;
            existedOrder.ShipName = updatedOrder.ShipName;
            existedOrder.ShipAddress = updatedOrder.ShipAddress;
            existedOrder.ShipCity = updatedOrder.ShipCity;
            existedOrder.ShipRegion = updatedOrder.ShipRegion;
            existedOrder.ShipPostalCode = updatedOrder.ShipPostalCode;
            existedOrder.ShipCountry = updatedOrder.ShipCountry;
            await context.SaveChangesAsync();
            return existedOrder;
        }
    }
}
