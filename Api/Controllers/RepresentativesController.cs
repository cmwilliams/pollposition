using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Config;
using MediatR;
using System.Threading;
using Api.Models.Queries;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentativesController : ControllerBase
    {
        private  IMediator _mediator;

        public RepresentativesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string address, CancellationToken cancellationtoken)
        {
            var viewModel = await _mediator.Send(new RepresentativeQuery { Address = address }, cancellationtoken);

            return Ok(new ApiOkResponse(viewModel));
        }
    }
}
