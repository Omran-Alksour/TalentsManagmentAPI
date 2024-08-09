using Domain.Shared;

namespace Domain.Errors
{
    public static class DomainErrors
    {
        public static class ValueObject
        {
            public static class Email
            {
                public static readonly Error MaxLengthExceeded = new("Email MaxLength", "Email is longer than allowed max length");
                public static readonly Error InvalidFormat = new("Email InvalidFormat", "The email format is invalid.");

            }
        }
    }
}