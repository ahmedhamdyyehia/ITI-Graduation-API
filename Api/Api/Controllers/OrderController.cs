using Api.DTOs;
using Api.Errors;
using AutoMapper;
using Core.Interfaces;
using Core.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService , IMapper mapper )
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {

            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Address address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await orderService.CreateOrderAsync(email, address, orderDto.DeliveryMethodId, orderDto.BasketId );

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }
    }
}
