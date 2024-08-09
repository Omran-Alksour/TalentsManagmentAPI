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


    }
}
