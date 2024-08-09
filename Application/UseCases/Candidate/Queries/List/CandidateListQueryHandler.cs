using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using Application.UseCases.Candidate.Responses;
using Domain.Repositories;
using Domain.Shared;
using Entities = Domain.Entities.Candidate ;

namespace Application.UseCases.Candidate.Queries.List
{
    public sealed class CandidateListQueryHandler : IQueryHandler<CandidateListQuery, PagedResponse<CandidateResponse>>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICacheService _cacheService;



        public CandidateListQueryHandler(ICandidateRepository candidateRepository , ICacheService cacheService)
        {
            _candidateRepository = candidateRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<PagedResponse<CandidateResponse>>> Handle(CandidateListQuery request, CancellationToken cancellationToken)
        {
            PagedResponse<Entities.Candidate>? candidates = await _candidateRepository.ListAsync(
                                request.PageNumber,
                                request.PageSize,
                                request.Search,
                                request.OrderBy,
                                request.OrderDirection,
                                cancellationToken
                              );



            if (candidates is null || candidates.Data == null || !candidates.Data.Any())
            {
                var emptyPagedResponse = new PagedResponse<CandidateResponse>(
                    new List<CandidateResponse>(),
                    request.PageNumber,
                    request.PageSize,
                    0 
                );

                return Result.Success(emptyPagedResponse);
            }

            var candidateResponses = candidates.Data.Select(candidate => new CandidateResponse(
                candidate.Id,
                candidate.FirstName,
                candidate.LastName,
                candidate.Email,
                candidate.Comment,
                candidate.PhoneNumber,
                candidate.CallTimeInterval,
                candidate.LinkedInProfileUrl,
                candidate.GitHubProfileUrl
            )).ToList();

            var pagedResponse = new PagedResponse<CandidateResponse>(
                candidateResponses,
                candidates.PageNumber,
                candidates.PageSize,
                candidates.TotalRecords
            );

            return Result.Success(pagedResponse);
        }
    }
}
