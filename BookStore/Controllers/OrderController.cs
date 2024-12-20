using AutoMapper;
using BookStore.DTOs.Author;
using BookStore.DTOs.Order;
using BookStore.DTOs.OrderItems;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        UnitOfWorks unit;
        IMapper mapper;
        public OrderController(UnitOfWorks unit, IMapper mapper)
        { 
            this.unit = unit;
            this.mapper = mapper;
        }
        [HttpGet("GetOrders")]
        [SwaggerOperation(Summary = "Get all orders", Description = "Retrieve all orders with their details.")]
        [SwaggerResponse(200, "Returns all orders", typeof(List<DisplayOrderDTO>))]
        public IActionResult GetOrders()
        {
            var orders = unit.OrderGenRepo.GetAll().ToList();

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No orders found." });
            }

            var orderDTOs = orders.Select(order =>
            {
                var orderItems = unit.OrderItemsGenRepo.GetAll()
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Select(oi =>
                    {
                        var book = unit.BookGenRepo.GetById(oi.BookId); 
                        decimal unitPrice = 0;
                        if (book != null)
                        {
                            unitPrice = book.Price;  
                        }
                        else
                        {
                            unitPrice = oi.UnitPrice; 
                        }

                        Console.WriteLine($"Book ID: {oi.BookId}, Retrieved Unit Price: {unitPrice}");

                        return new DisplayOrderItemsDTO
                        {
                            BookId = oi.BookId,
                            UnitPrice = unitPrice, 
                            Quantity = oi.Quantity
                        };
                    })
                    .ToList();

                decimal updatedTotalPrice = orderItems.Sum(item => item.UnitPrice * item.Quantity);

                return new DisplayOrderDTO
                {
                    OrderId = order.OrderId,
                    CustomertId = order.CustomertId.ToString(),
                    Date = order.Date,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = updatedTotalPrice, 
                    items = orderItems
                };
            }).ToList();

            return Ok(orderDTOs);
        }





        [HttpPost("AddOrder")]
        [SwaggerOperation(Summary = "add new order")]
        [SwaggerResponse(201, "if order created succcesfully")]
        [SwaggerResponse(400, "ifinvalid order data")]
        public IActionResult Add(AddOrderDTO addOrderDTO) 
        {
            Order order = new Order()
            {
                CustomertId = addOrderDTO.CustomertId,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day),
                OrderStatus="created",
            };
            unit.OrderGenRepo.Add(order);
            unit.save();
            decimal totalPrice= 0;
            foreach (var item in addOrderDTO.items)
            {
                Book b = unit.BookGenRepo.GetById(item.BookId);
                totalPrice = totalPrice + (b.Price * item.Quantity);
                OrderItems orderItems = new OrderItems() 
                {
                    OrderId=order.OrderId,
                    BookId=item.BookId,
                    Quantity=item.Quantity,
                    UnitPrice=b.Price,
                };
                if (b.Stock > orderItems.Quantity) 
                {
                    unit.OrderItemsGenRepo.Add(orderItems);
                    b.Stock -= item.Quantity;
                    unit.BookGenRepo.Edit(b);
                }
                else 
                {
                    return BadRequest("Invaild quantity");
                }
            }
            order.TotalPrice= totalPrice;
            unit.OrderGenRepo.Edit(order);
            unit.save();
            return Ok();

        }






        [HttpPut("EditOrder/{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Edit order data (Admin Only)",
        Description = "This endpoint allows administrators to edit order data. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(204, "If order updated successfully")]
        [SwaggerResponse(400, "If invalid order data")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]
        public IActionResult EditOrder(int id, AddOrderDTO updatedOrderDTO)
        {
            var order = unit.OrderGenRepo.GetById(id);

            if (order == null)
            {
                return NotFound(new { Message = $"Order with ID {id} not found." });
            }

            order.CustomertId = updatedOrderDTO.CustomertId;
            order.Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            order.OrderStatus = "Updated";

            var existingOrderItems = unit.OrderItemsGenRepo.GetAll()
                .Where(n => n.OrderId == id).ToList();

            foreach (var item in updatedOrderDTO.items)
            {
                var book = unit.BookGenRepo.GetById(item.BookId);
                if (book == null)
                {
                    return BadRequest(new { Message = $"Book with ID {item.BookId} not found." });
                }

                var existingItem = existingOrderItems.FirstOrDefault(m => m.BookId == item.BookId);
                if (existingItem != null)
                {
                    if (item.UnitPrice != book.Price)
                    {
                        return BadRequest(new { Message = "Unit price cannot be changed manually. It should match the book price." });
                    }

                    if (book.Stock + existingItem.Quantity < item.Quantity)
                    {
                        return BadRequest(new { Message = $"Insufficient stock for Book ID {item.BookId}." });
                    }

                    book.Stock += existingItem.Quantity;
                    book.Stock -= item.Quantity;

                    existingItem.Quantity = item.Quantity;
                    existingItem.UnitPrice = book.Price;  

                    unit.OrderItemsGenRepo.Edit(existingItem);
                    unit.BookGenRepo.Edit(book);
                }
                else
                {
                    if (book.Stock < item.Quantity)
                    {
                        return BadRequest(new { Message = $"Insufficient stock for Book ID {item.BookId}." });
                    }

                    var newOrderItem = new OrderItems
                    {
                        OrderId = id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price,  
                    };
                    book.Stock -= item.Quantity;

                    unit.OrderItemsGenRepo.Add(newOrderItem);
                    unit.BookGenRepo.Edit(book);
                }
            }

            order.TotalPrice = updatedOrderDTO.items.Sum(item =>
            {
                var book = unit.BookGenRepo.GetById(item.BookId);
                if (book == null)
                {
                    return 0;
                }
                return book.Price * item.Quantity; 
            });

            unit.OrderGenRepo.Edit(order);
            unit.save();

            return Ok(new { Message = $"Order with ID {id} updated successfully.", TotalPrice = order.TotalPrice, Status = order.OrderStatus });
        }


    }
}
