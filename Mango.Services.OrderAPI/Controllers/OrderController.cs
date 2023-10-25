using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dtos;
using Mango.Services.OrderAPI.Services.IServices;
using Mango.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private IProductRestService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderController(ApplicationDbContext db,
            IProductRestService productService, IMapper mapper, IConfiguration configuration
            , IMessageBus messageBus)
        {
            _db = db;
            _messageBus = messageBus;
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderHeaderDto>> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeaderDto);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetailsDtos = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetailsDtos);
                orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);
                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;

                return Ok(orderHeaderDto);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<StripeRequestDto>> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {

            var options = new SessionCreateOptions
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            foreach (var item in stripeRequestDto.OrderHeaderDto.OrderDetailsDtos) 
            {
                var sessionLineItem = new SessionLineItemOptions 
                {
                    PriceData = new SessionLineItemPriceDataOptions 
                    {
                        UnitAmount = (long) (item.Price *100), // if price 20$ , the unitAmout will be 20.00
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions 
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            stripeRequestDto.StripeSessionUrl = session.Url;
            

            OrderHeader orderHeader = _db.OrderHeaders.First(x=>x.OrderHeaderId == stripeRequestDto.OrderHeaderDto.OrderHeaderId);

            orderHeader.StripeSessionId = session.Id;
            _db.SaveChanges();

            return Ok(stripeRequestDto);
        }
    }
}
