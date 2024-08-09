using Application.Abstractions.Caching;
using Application.UseCases.Candidate.Commands.Create;
using Domain.Abstractions;
using Domain.Entities.Candidate;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Candidates.Commands;

public class CandidateCreateOrUpdateCommandHandlerTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public CandidateCreateOrUpdateCommandHandlerTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCommandIsValid()
    {
        // Arrange
        var email = Email.Create("omranalksour@gmail.com").Value;
        var command = new CandidateCreateOrUpdateCommand(
            "Omran",
            "Alksour",
            email,
            "Comment",
            "+962789079890",
            DateTime.Now,
            "https://www.linkedin.com/in/omran-alksour",
            "https://github.com/omran-Alksour"
        );

        _candidateRepositoryMock
            .Setup(repo => repo.CreateOrUpdateAsync(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Comment,
                command.PhoneNumber,
                command.CallTimeInterval,
                command.LinkedInProfileUrl,
                command.GitHubProfileUrl,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(Result.Success(new Candidate(
                Guid.NewGuid(),
                command.FirstName,
                command.LastName,
                command.Email.Value,
                command.Comment,
                command.PhoneNumber,
                command.CallTimeInterval,
                command.LinkedInProfileUrl,
                command.GitHubProfileUrl
            )));

        var handler = new CandidateCreateOrUpdateCommandHandler(
            _candidateRepositoryMock.Object,
            _cacheServiceMock.Object
        );

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
    }

}