using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.UseCases.Candidate.Commands.Create
{
    public sealed record CandidateCreateOrUpdateCommand(
        string FirstName,
        string LastName,
        Email Email,
        string Comment,
        string? PhoneNumber = null,
        DateTime? CallTimeInterval = null,
        string? LinkedInProfileUrl = null,
        string? GitHubProfileUrl = null,
        CancellationToken CancellationToken = default
    ) : ICommand<Guid>;
}
