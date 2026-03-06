using EMS.Core.DTOs.Payment;
using EMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService<Registration> _paymentService;

        public PaymentController(IPaymentService<Registration> paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreateIntent(PaymentRequest request)
        {
            var response = await _paymentService
                .CreateOrUpdateIntentAsync(request.EntityId);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }
    }
}