
using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class CandidateRepository : BaseRepository, ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

    }
}
