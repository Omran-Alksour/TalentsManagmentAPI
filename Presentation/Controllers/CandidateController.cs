using Application.UseCases.Candidate.Commands.Create;
using Application.UseCases.Candidate.Commands.Delete;
using Application.UseCases.Candidate.Queries.GetByEmail;
using Application.UseCases.Candidate.Queries.List;
using Domain.Requests;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers
{
    [AllowAnonymous]
    [Route("api/Candidate")]
    public class CandidateController : ApiController
    {
        public CandidateController(ISender sender) : base(sender)
        {
        }


        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(email);
            if (!emailResult.IsSuccess)
            {
                return BadRequest(emailResult.Error);
            }

            var query = new CandidateGetByEmailQuery(emailResult);
            var result = await Sender.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : StatusCode(600, result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 15, string? search = null, string? orderBy = "Email", string? orderDirection = "asc", CancellationToken cancellationToken = default)
        {
            var query = new CandidateListQuery(pageNumber, pageSize, search, orderBy, orderDirection, cancellationToken);
            var result = await Sender.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : StatusCode(600, result.Error);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromForm] CandidateCreateOrUpdateRequest request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            if (!emailResult.IsSuccess)
            {
                return BadRequest(emailResult.Error);
            }

            var command = new CandidateCreateOrUpdateCommand(
                request.FirstName,
                request.LastName,
                Email.Create(request.Email),
                request.Comment,
                request.PhoneNumber,
                request.CallTimeInterval,
                request.LinkedInProfileUrl,
                request.GitHubProfileUrl,
                cancellationToken
            );
            var result = await Sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : StatusCode(600, result.Error);
        }


        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email, bool forceDelete = false, CancellationToken cancellationToken = default)
        {
            var emailResult = Email.Create(email);
            if (!emailResult.IsSuccess)
            {
                return BadRequest(emailResult.Error);
            }

            var command = new CandidateDeleteCommand(Email.Create(email), forceDelete, cancellationToken);
            var result = await Sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : StatusCode(600, result.Error);
        }
    }
}
