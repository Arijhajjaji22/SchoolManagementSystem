using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Services;
using Stripe.Checkout;

namespace ProjetBrima.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ApplicationDbContext _context;
        public PaymentController(IPaymentService paymentService, ApplicationDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession([FromBody] AmountDto request)
        {
            long amount = request.Amount;
            int eleveId = request.EleveId;  // Récupérer l'eleveId du DTO

            var session = _paymentService.CreateCheckoutSession(amount, eleveId);  // Passer les deux paramètres
            return Ok(new { id = session.Id });
        }
    

    }
}
