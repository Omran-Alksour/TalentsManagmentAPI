using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Entities = Domain.Entities.Candidate;

namespace Application.UseCases.Candidate.Commands.Create
{
    public sealed class CandidateCreateOrUpdateCommandHandler : ICommandHandler<CandidateCreateOrUpdateCommand, Guid>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICacheService _cacheService;

        public CandidateCreateOrUpdateCommandHandler(ICandidateRepository candidateRepository, ICacheService cacheService)
        {
            _candidateRepository = candidateRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<Guid>> Handle(CandidateCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {

            Result<Entities.Candidate?> candidate = await _candidateRepository.CreateOrUpdateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Comment,
                request.PhoneNumber,
                request.CallTimeInterval,
                request.LinkedInProfileUrl,
                request.GitHubProfileUrl,
                cancellationToken
            );


            if (candidate is null)
            {
                return Result.Failure<Guid>(ApplicationErrors.Candidates.Commands.CandidateCreationOrUpdateFailed);
            }

          await  _cacheService.SetAsync($"{nameof(Entities.Candidate)}_{request.Email.Value.Trim().ToLower()}", candidate.Value);


            return Result.Success<Guid>(Guid.Parse(candidate?.Value?.Id));
        }
    }
}
