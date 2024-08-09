using Application.Abstractions.Caching;
using Application.UseCases.Candidate.Commands.Delete;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Candidates.Commands;

public class CandidateDeleteCommandHandlerTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public CandidateDeleteCommandHandlerTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _cacheServiceMock = new Mock<ICacheService>();

    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailExists()
    {
        // Arrange
        var email = Email.Create("omranalksour@mail.com").Value;
        var command = new CandidateDeleteCommand(email, false);
        var candidateId = Guid.NewGuid();
        var deletionResult = Result.Success(candidateId);

        _candidateRepositoryMock
            .Setup(repo => repo.DeleteAsync(email, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletionResult);

        var handler = new CandidateDeleteCommandHandler(
            _candidateRepositoryMock.Object,
            _cacheServiceMock.Object
        );

        // Act
        Result<Guid> handlerResult = await handler.Handle(command, default);


        // Assert
        handlerResult.IsSuccess.Should().BeTrue();
        handlerResult.Value.Should().Be(candidateId);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = Email.Create("doesNotExist@gmail.com").Value;
        var command = new CandidateDeleteCommand(email, false);
        var result = Result.Failure<Guid>(ApplicationErrors.Candidates.Commands.CandidateNotFound);

        _candidateRepositoryMock
            .Setup(repo => repo.DeleteAsync(email, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var handler = new CandidateDeleteCommandHandler(
            _candidateRepositoryMock.Object,
             _cacheServiceMock.Object

        );

        // Act
        var handlerResult = await handler.Handle(command, default);

        // Assert

        handlerResult.IsSuccess.Should().BeFalse();
        handlerResult.Error.Should().Be(ApplicationErrors.Candidates.Commands.CandidateNotFound);
    }
}
