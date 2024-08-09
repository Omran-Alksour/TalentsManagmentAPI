using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Caching;
using Application.UseCases.Candidate.Queries.List;
using Domain.Abstractions;
using Domain.Entities.Candidate;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.UnitTests.Candidates.Queries;

public class CandidateListQueryHandlerTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public CandidateListQueryHandlerTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WithFilteredPagedCandidates_WhenSearchTermIsProvided()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var search = "Omran";  // Search parameter
        var orderBy = "lastName";
        var orderDirection = "asc";

        var query = new CandidateListQuery(pageNumber, pageSize, search, orderBy, orderDirection);

        var candidates = new List<Candidate>
        {
            new Candidate(
                Guid.NewGuid(),
                "Omran",
                "Alksour",
                "omran@mail.com",
                "Comment1"
            ),
            new Candidate(
                Guid.NewGuid(),
                "OtherFirstName",
                "OtherLastName",
                "other@mail.com",
                "Comment2"
            )
        };

        var filteredCandidates = candidates.Where(c =>
                 c.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                 c.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                 c.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                 c.Comment.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        var totalRecords = filteredCandidates.Count;
        var pagedResponse = new PagedResponse<Candidate>(
            filteredCandidates,
            pageNumber,
            pageSize,
            totalRecords
        );

        _candidateRepositoryMock
            .Setup(repo => repo.ListAsync(pageNumber, pageSize, search, orderBy, orderDirection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResponse);

        var handler = new CandidateListQueryHandler(
            _candidateRepositoryMock.Object,
            _cacheServiceMock.Object
        );

        // Act
        var handlerResult = await handler.Handle(query, default);

        // Assert
        handlerResult.IsSuccess.Should().BeTrue();
        handlerResult.Value.Data.Should().HaveCount(1);

        handlerResult.Value.Data.Select(c => c.Email)
      .Should().BeEquivalentTo(pagedResponse.Data.Select(c => c.Email));




    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WithEmptyList_WhenNoCandidates()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        string? search = null; 
        var orderBy = "lastName";
        var orderDirection = "asc";

        var query = new CandidateListQuery(pageNumber, pageSize, search, orderBy, orderDirection);

        var candidates = new List<Candidate>();

        var totalRecords = 0; 
        var pagedResponse = new PagedResponse<Candidate>(
            candidates,
            pageNumber,
            pageSize,
            totalRecords
        );

        _candidateRepositoryMock
            .Setup(repo => repo.ListAsync(pageNumber, pageSize, search, orderBy, orderDirection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResponse);

        var handler = new CandidateListQueryHandler(
            _candidateRepositoryMock.Object,
            _cacheServiceMock.Object
        );

        // Act
        var handlerResult = await handler.Handle(query, default);

        // Assert
        handlerResult.IsSuccess.Should().BeTrue();
        handlerResult.Value.Should().BeEquivalentTo(pagedResponse);
    }

}
