using Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace Domain.Entities.Candidate
{
    public class Candidate : IdentityUser, IDeletable
    {



        public Candidate() { }

        public Candidate(Guid id, string firstName, string lastName, string email, string comment,
            string? phoneNumber = null,
            DateTime? callTimeInterval = null, string? linkedInProfileUrl = null, string? gitHubProfileUrl = null)
        {
            Id = id.ToString();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Comment = comment;
            PhoneNumber = phoneNumber;
            CallTimeInterval = callTimeInterval;
            LinkedInProfileUrl = linkedInProfileUrl;
            GitHubProfileUrl = gitHubProfileUrl;

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public DateTime? CallTimeInterval { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? GitHubProfileUrl { get; set; }

        public bool IsDeleted { get; private set; }


        public void Delete()
        {
            IsDeleted = true;
        }

        public void UnDelete()
        {
            IsDeleted = false;
        }


    }
}
