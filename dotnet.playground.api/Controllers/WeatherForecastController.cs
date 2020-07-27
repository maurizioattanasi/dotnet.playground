using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet.playground.domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet.playground.api.controllers
{
    /// <summary>
    /// Fake Weather Forecast controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Weather Forecast controller
        /// </summary>
        /// <param name="mediator"></param>
        public WeatherForecastController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns all the weather forcasts
        /// </summary>
        /// <param name="page">Index of the first element to return</param>
        /// <param name="pageSize">Number of elements to return</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAll(int? page = null, int? pageSize = null)
        {
            try
            {
                var forecasts = await this.mediator.Send(new Features.Weather.GetAll(page, pageSize));

                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var problemDetails = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = content,
                    Instance = $"/Weather/GetAll/",
                };

                return BadRequest(problemDetails);
            }
        }
    }
}
