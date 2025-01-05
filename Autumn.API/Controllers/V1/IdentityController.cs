using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.API.Contract.V1;
using Autumn.API.Contract.V1.Requests;
using Autumn.API.Contract.V1.Responses;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Autumn.API.V1
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public IdentityController(IdentityService identityService) {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse { Error = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)) });
            }

            var authResponse = _identityService.Register(request.Username, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Error = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public IActionResult Login([FromForm] UserRequest request)
        {
            var authResponse =  _identityService.Login(request.Username, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Error = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }
    }
}