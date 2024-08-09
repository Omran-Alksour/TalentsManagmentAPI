using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using Entities = Domain.Entities.Candidate;

namespace Application.UseCases.Candidate.Commands.Delete
{
    public sealed class CandidateDeleteCommandHandler : ICommandHandler<CandidateDeleteCommand, Guid>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICacheService _cacheService;

        public CandidateDeleteCommandHandler(ICandidateRepository candidateRepository, ICacheService cacheService)
        {
            _candidateRepository = candidateRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<Guid>> Handle(CandidateDeleteCommand request, CancellationToken cancellationToken)
        {

            await _cacheService.RemoveAsync($"{nameof(Entities.Candidate)}_{request.Email.Value.Trim().ToLower()}");

            return await _candidateRepository.DeleteAsync(request.Email, request.ForceDelete, cancellationToken);
        }
    }
}
