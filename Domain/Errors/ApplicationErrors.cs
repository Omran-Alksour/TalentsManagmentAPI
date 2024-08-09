using Domain.Shared;

namespace Domain.Errors;

public static class ApplicationErrors
{

    public static class Candidates
    {
        public static class Email
        {
            public static readonly Error InvalidFormat = new("Email InvalidFormat", "The email format is invalid.");
        }

        public static class Queries
        {
            public static readonly Error CandidateNotFound = new("Candidate Not Found", "No such candidate in the cache or database");
        }

        public static class Commands
        {
            public static readonly Error CandidateCreationOrUpdateFailed = new("Candidate Creation Failed", "Failed to create or update the candidate");
            public static readonly Error CandidateNotFound = new("Candidate Not Found", "Candidate Not Found");
        }
    }





    public static class Users
    {
        public static class Queries
        {
            public static readonly Error UserNotFound = new("User Not Found", "No Such user in the database");
        }
    }
    public static class Languages
    {
        public static class Queries
        {
            public static readonly Error LanguageNotFound = new("Language Not Found", "No Such Language in the database");
            public static readonly Error NoItemsFound = new("No Language Found", "No Such Language in the database");
        }
    }

}