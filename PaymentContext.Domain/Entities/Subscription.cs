using Flunt.Validations;
using PaymentContext.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentContext.Domain.Entities
{
    public class Subscription : Entity
    {
        private IList<Payment> _payments;

        public Subscription(DateTime? expireDate)
        {
            CreateDate = DateTime.Now;
            LastUptdateDate = DateTime.Now;
            ExpirateDate = expireDate;
            Active = true;
            _payments = new List<Payment>();
        }

        public DateTime CreateDate { get; private set; }
        public DateTime LastUptdateDate { get; private set; }
        public DateTime? ExpirateDate { get; private set; }
        public bool Active { get; private set; }
        public IReadOnlyCollection<Payment> Payments { get 
            {
                return _payments.ToArray();
            } 
        }

        public void AddPayment(Payment payment)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "Data deve ser futura")
            );

            //if (Valid)
            //{
            //    _payments.Add(payment);
            //}

            _payments.Add(payment);
        }

        public void Activate()
        {
            Active = true;
            LastUptdateDate = DateTime.Now;
        }

        public void Inactivate()
        {
            Active = false;
            LastUptdateDate = DateTime.Now;
        }
    }
}
