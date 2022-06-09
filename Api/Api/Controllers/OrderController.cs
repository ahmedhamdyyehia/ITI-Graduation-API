using Api.DTOs;
using Api.Errors;
using API.Dtos;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
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

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {

            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Address address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await orderService.CreateOrderAsync(email, address, orderDto.DeliveryMethodId, orderDto.BasketId);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<OrderToReturnDto>> GetOredersForUser()
        {
            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var orders = await orderService.GetOrdersByUSerAsync(email);
            return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("userOrder/{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id )
        {
            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var order = await orderService.GetOrderByIdAsync(id , email);
            if (order == null)
                return NotFound(new ApiResponse(404));

            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethodsAsync());
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new ApiResponse(404));

            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> AcceptOrder(int id)
        {          
            return Ok(await orderService.UpdateOrderStatus(id));
        }


        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderStatistics>> GetOrderStatictics()
        {
            return Ok(await orderService.GetOrderStatistics());
        }

        [HttpGet("Brand/statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BrandStatistics>> GetBrandsStatictics()
        {
            return Ok(await orderService.GetBrandsStatistics());
        }
    }
}
