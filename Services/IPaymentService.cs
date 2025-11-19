using Stripe.Checkout;

namespace ProjetBrima.Services

{
    public interface IPaymentService
    {
        public Session CreateCheckoutSession(long amount, int eleveId);

      

    }
}
