namespace Application.UseCases.Candidate.Responses
{
    public sealed record CandidateResponse(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string Comment,
        string? PhoneNumber = null,
        DateTime? CallTimeInterval = null,
        string? LinkedInProfileUrl = null,
        string? GitHubProfileUrl = null
    );
}
