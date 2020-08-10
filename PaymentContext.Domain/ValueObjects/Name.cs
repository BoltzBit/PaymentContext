using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firsName, string lastName)
        {
            FirstName = firsName;
            LastName = lastName;

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, "Name.FirstName", "Nome deve conter pelo menos 3 caracteres")
                .HasMinLen(LastName, 3, "Name.LastName","Sobrenome precisa ter pelo menos 3 caracteres")
                .HasMaxLen(FirstName, 40, "Name.FirstName", "Nome precisa ter no máximo 40 caracteres")
            );
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}
