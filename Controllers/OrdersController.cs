﻿using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebService.Model;
using WebService.Services;

namespace WebService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : Controller
{
    private readonly NorthwindContext context;
    private readonly IOrdersRepository repository;
    private readonly IOrderDetailsRepository orderDetailsRepository;

    public OrdersController(IOrdersRepository repository, IOrderDetailsRepository orderDetailsRepository, NorthwindContext context)
    {
        this.repository = repository;
        this.orderDetailsRepository = orderDetailsRepository;
        this.context = context;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
    public IActionResult GetAllOrders() => Ok(repository.GetAllOrders());


    [HttpGet("{id}", Name = nameof(GetOrderByID))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetOrderByID(int? id) 
    {
        var existingOrder = repository.GetOrderByID(id);
        if (existingOrder == null) return NotFound();
        return Ok(existingOrder);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Order))]
    public IActionResult CreateNewOrder([FromBody] Order newOrder)
    {
        repository.CreateNewOrder(newOrder);
        return CreatedAtAction(nameof(GetOrderByID), new { id = newOrder.OrderId }, newOrder);
    }
}