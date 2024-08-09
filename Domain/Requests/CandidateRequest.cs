using System.ComponentModel.DataAnnotations;

namespace Domain.Requests;

    public sealed class CandidateCreateOrUpdateRequest
    {
        [Required(ErrorMessage = "The First Name field is required.")]
        public string FirstName { get; set; } =string.Empty;

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Comment field is required.")]
        public string Comment { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public DateTime? CallTimeInterval { get; set; } = DateTime.Now;

        [Url(ErrorMessage = "The LinkedIn profile URL is not valid.")]
        public string? LinkedInProfileUrl { get; set; } = string.Empty;

    [Url(ErrorMessage = "The GitHub profile URL is not valid.")]
        public string? GitHubProfileUrl { get; set; } = string.Empty;
}



public sealed class CandidateByEmailRequest
{
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
}
