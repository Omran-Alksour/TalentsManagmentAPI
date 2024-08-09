using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using Application.UseCases.Candidate.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Entities = Domain.Entities.Candidate;


namespace Application.UseCases.Candidate.Queries.GetByEmail
{
    public sealed class CandidateGetByEmailQueryHandler : IQueryHandler<CandidateGetByEmailQuery, CandidateResponse>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICacheService _cacheService;

        public CandidateGetByEmailQueryHandler(ICandidateRepository candidateRepository, ICacheService cacheService)
        {
            _candidateRepository = candidateRepository;
            _cacheService = cacheService;

        }

        public async Task<Result<CandidateResponse>> Handle(CandidateGetByEmailQuery request, CancellationToken cancellationToken)
        {

            Entities.Candidate? candidate = await _cacheService.GetAsync<Entities.Candidate>($"{nameof(Entities.Candidate)}_{request.Email.Value.Trim().ToLower()}",
                            async () =>
                            {
                                var value = await _candidateRepository.GetByEmailAsync(request.Email, cancellationToken);
                                return value;
                            }
                            ,
                          cancellationToken
                            );

            if (candidate is null)
            {
                return Result.Failure<CandidateResponse>(ApplicationErrors.Candidates.Queries.CandidateNotFound);
            }

            CandidateResponse candidateDetails = new(
                candidate.Id,
                candidate.FirstName,
                candidate.LastName,
                candidate.Email,
                candidate.Comment,
                candidate.PhoneNumber,
                candidate.CallTimeInterval,
                candidate.LinkedInProfileUrl,
                candidate.GitHubProfileUrl
            );

            return Result.Success(candidateDetails);
        }
    }
}
