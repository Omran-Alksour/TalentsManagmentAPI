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

    }
}
