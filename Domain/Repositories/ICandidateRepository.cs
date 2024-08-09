

using Domain.Entities.Candidate;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface ICandidateRepository
{

    Task<Candidate?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);


    Task<PagedResponse<Candidate>?> ListAsync(int? pageNumber = null, int? pageSize = null, string? search = null, string? orderBy = null, string? orderDirection = "asc", CancellationToken cancellationToken = default);

    Task<Result<Candidate?>> CreateOrUpdateAsync(
    string firstName,
    string lastName,
    Email email,
    string comment,
    string? phoneNumber = null,
    DateTime? callTimeInterval = null,
    string? linkedInProfileUrl = null,
    string? gitHubProfileUrl = null,
    CancellationToken cancellationToken = default);

    Task<Result<Candidate?>> UpdateAsync(
    Guid id,
    string firstName,
    string lastName,
    Email email,
    string comment,
    string? phoneNumber = null,
    DateTime? callTimeInterval = null,
    string? linkedInProfileUrl = null,
    string? gitHubProfileUrl = null,
    CancellationToken cancellationToken = default);

    Task<Result<Guid>> DeleteAsync(Email email, bool forceDelete, CancellationToken cancellationToken = default);


}
