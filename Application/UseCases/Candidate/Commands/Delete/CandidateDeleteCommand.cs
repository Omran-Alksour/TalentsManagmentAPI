using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.UseCases.Candidate.Commands.Delete
{
    public sealed record CandidateDeleteCommand(
        Email Email,
        bool ForceDelete = false,
        CancellationToken CancellationToken = default
    ) : ICommand<Guid>;
}
