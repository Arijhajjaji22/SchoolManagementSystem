using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Models;
using Stripe;
using Stripe.Checkout;
namespace ProjetBrima.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly ApplicationDbContext _context;


        public PaymentService(IOptions<StripeSettings> stripeSettings, ApplicationDbContext context)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _context = context;
        }

        public Session CreateCheckoutSession(long amount, int eleveId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "eur",
                    UnitAmount = amount * 100, // Convertir en centimes
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Frais d'inscription" // Ou tout autre produit que vous souhaitez facturer
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = "http://localhost:5108/Home/Success",
                CancelUrl = "http://localhost:5108/Home/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Enregistrer le paiement dans la base de données
            var paiement = new Paiement
            {
                EleveId = eleveId,
                Montant = amount,
                EstPayé = true, // Paiement non effectué encore
                DatePaiement = DateTime.Now

            };
            _context.Paiements.Add(paiement);
            _context.SaveChanges();

            return session;
        }




    }
}