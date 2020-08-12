using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Repositories;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Entities;
using System;
using PaymentContext.Domain.Services;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : 
        Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar o cadastro!");
            }

            if (_repository.DocumentExists(command.Document)) {
                AddNotification("Document","Este cpf já existe");
            }

            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Email","Este email já existe");
            }

            var name = new Name(command.FirstName, command.LastName);
            var email = new Email(command.Email);
            var document = new Document(command.Document, EDocumentType.CPF);
            var address = new Address(command.Street, command.Neighborhood, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);


            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, email, address, student, subscription, payment);

            if (Invalid)
            {
                return new CommandResult(false, "Não foi possivel realizar sua assinatura");
            }

            _repository.CreateSubscription(student);
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao Balta io", "Sua assinatura foi criada");



            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Documento ja existe em nossa base!");

            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Email já existe!");

            var name = new Name(command.FirstName, command.LastName);
            var email = new Email(command.Email);
            var document = new Document(command.Document, EDocumentType.CPF);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.State, command.City, command.Country, command.ZipCode);

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, email, address, student, subscription, payment);

            if (Invalid)
                return new CommandResult(false, "não foi possivel realizar a inscrição");

            _repository.CreateSubscription(student);
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo!", "Sua incrição foi efetivada");

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}
