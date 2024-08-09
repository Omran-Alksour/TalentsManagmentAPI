using Application.Abstractions.Messaging;
using Application.UseCases.Candidate.Responses;
using Domain.Shared;

namespace Application.UseCases.Candidate.Queries.List
{
public sealed record CandidateListQuery(
    int PageNumber = 1,
    int PageSize = 15,
    string? Search = null,
    string? OrderBy = null,
    string? OrderDirection = "asc",
    CancellationToken CancellationToken = default
) : IQuery<PagedResponse<CandidateResponse>>;}
