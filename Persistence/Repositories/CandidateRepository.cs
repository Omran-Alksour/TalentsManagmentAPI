using Domain.Entities.Candidate;
using Domain.Errors;
using Domain.Extensions;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;



namespace Persistence.Repositories
{
    public sealed class CandidateRepository : BaseRepository, ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public async Task<Candidate?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Candidates.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == email.Value.ToLower().Trim() && !c.IsDeleted, cancellationToken);
        }


        public async Task<PagedResponse<Candidate>> ListAsync(int? pageNumber = null, int? pageSize = null, string? search = null, string? orderBy = null, string? orderDirection = "asc", CancellationToken cancellationToken = default)
        {
            IQueryable<Candidate> query = _context.Candidates.AsNoTracking()
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.FirstName, $"%{search}%") ||
                    EF.Functions.Like(c.LastName, $"%{search}%") ||
                    EF.Functions.Like(c.Email, $"%{search}%") ||
                    EF.Functions.Like(c.Comment, $"%{search}%"));
            }


            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.AsQueryable().OrderByDynamic(orderBy, orderDirection);
            }
            else
            {
                query = query.OrderBy(s => s.Email);
            }

            return await Pagination<Candidate>.GetWithOffsetPagination(query, pageNumber, pageSize, cancellationToken);
        }

        public async Task<Result<Candidate?>> CreateOrUpdateAsync(string firstName, string lastName, Email email, string comment, string? phoneNumber = null, DateTime? callTimeInterval = null, string? linkedInProfileUrl = null, string? gitHubProfileUrl = null, CancellationToken cancellationToken = default)
        {

            Candidate? candidate = await _context.Candidates.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email.Trim().ToLower() == email.Value.Trim().ToLower() && !c.IsDeleted, cancellationToken);


            if (candidate is null)
            {
                candidate = new Candidate(Guid.NewGuid(), firstName, lastName, email.Value, comment, phoneNumber, callTimeInterval, linkedInProfileUrl, gitHubProfileUrl);
                var entityEntry = await _context.Set<Candidate>().AddAsync(candidate, cancellationToken);
                _ = await _context.SaveChangesAsync(cancellationToken);

                candidate = entityEntry.Entity;
            }
            else
            {
                candidate = await UpdateAsync(Guid.Parse(candidate.Id), firstName, lastName, email, comment, phoneNumber, callTimeInterval, linkedInProfileUrl, gitHubProfileUrl, cancellationToken);
            }

            return Result<Candidate?>.Success(candidate);
        }


        public async Task<Result<Candidate?>> UpdateAsync(Guid id, string firstName, string lastName, Email email, string comment, string? phoneNumber = null, DateTime? callTimeInterval = null, string? linkedInProfileUrl = null, string? gitHubProfileUrl = null, CancellationToken cancellationToken = default)
        {
            Candidate? candidate = await _context.Candidates
               .SingleOrDefaultAsync(c => c.Id == id.ToString() && c.Email.ToLower().Trim() == email.Value.ToLower().Trim() && !c.IsDeleted, cancellationToken);

            IQueryable<Candidate> query = _context.Candidates.AsNoTracking()
             .Where(c => !c.IsDeleted);

            var users = query.ToList();
            if (candidate is null)
                return Result.Failure<Candidate?>(ApplicationErrors.Candidates.Commands.CandidateCreationOrUpdateFailed);


            candidate.FirstName = firstName;
            candidate.LastName = lastName;
            candidate.Email = email.Value;
            candidate.Comment = comment;
            candidate.PhoneNumber = phoneNumber;
            candidate.CallTimeInterval = callTimeInterval;
            candidate.LinkedInProfileUrl = linkedInProfileUrl;
            candidate.GitHubProfileUrl = gitHubProfileUrl;

            _ = _context.Candidates.Update(candidate);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(candidate);
        }




        public async Task<Result<Guid>> DeleteAsync(Email email, bool forceDelete, CancellationToken cancellationToken = default)
        {
            Candidate? candidate = await _context.Candidates.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == email.Value.ToLower().Trim() && !c.IsDeleted, cancellationToken);

            if (candidate is null)
            {
                return Result.Failure<Guid>(ApplicationErrors.Candidates.Commands.CandidateNotFound);
            }


            if (forceDelete)
            {
                _context.Candidates.Remove(candidate);
            }
            else
            {
                candidate.Delete();
                _context.Candidates.Update(candidate);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(Guid.Parse(candidate.Id));
        }

    }
}
