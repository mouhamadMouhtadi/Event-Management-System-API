using EMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeEvent = Stripe.Event;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/stripe-webhook")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IPaymentService<Registration> _paymentService;

        public StripeWebhookController(IPaymentService<Registration> paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var sig = Request.Headers["Stripe-Signature"];
            var endpointSecret = "whsec_ab4da82c4cbbc50a4a14500eb82f53f6e803f645b4ca591df08d9709dcae494e";

            StripeEvent stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, sig, endpointSecret);
            }
            catch
            {
                return BadRequest();
            }

            var intent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == "payment_intent.succeeded")
                await _paymentService.HandlePaymentSucceeded(intent.Id);

            if (stripeEvent.Type == "payment_intent.payment_failed")
                await _paymentService.HandlePaymentFailed(intent.Id);

            return Ok();
        }
    }
}
