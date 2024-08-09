using Application.Abstractions.Messaging;
using Application.UseCases.Candidate.Responses;
using Domain.ValueObjects;

namespace Application.UseCases.Candidate.Queries.GetByEmail
{
    public sealed record CandidateGetByEmailQuery(
        Email Email
    ) : IQuery<CandidateResponse>;
}
