using Anshan.Domain;

namespace Ordering.Domain;

public class Address : ValueObject
{
    public string RecipientFullName { get; private set; }

    public string RecipientPhoneNumber { get; private set; }

    public string Line { private set; get; }

    public string PostalCode { get; private set; }

    public Address(string line, string recipientFullName, string recipientPhoneNumber, string postalCode)
    {
        RecipientFullName = recipientFullName;
        RecipientPhoneNumber = recipientPhoneNumber;
        Line = line;
        PostalCode = postalCode;
    }
}