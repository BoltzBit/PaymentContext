using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class CreditCardPayment : Payment
    {
        public CreditCardPayment(
            string cardHolderName, 
            string cardNumber, 
            string lasTransactionsNumber, 
            DateTime paiDate, 
            DateTime expireDate, 
            decimal total, 
            decimal totalPaid, 
            string payer, 
            Document document, 
            Address address, 
            Email email) : 
            base(
                paiDate, 
                expireDate, 
                total, 
                totalPaid, 
                payer, 
                document, 
                address, 
                email)
        {
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            LastTransactionNumber = lasTransactionsNumber;
        }

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string LastTransactionNumber { get; set; }
    }
}
