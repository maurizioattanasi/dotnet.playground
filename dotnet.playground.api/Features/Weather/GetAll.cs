using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace dotnet.playground.api.Features.Weather
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAll : IRequest<IEnumerable<domain.Entities.WeatherForecast>>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly int? page;
        private readonly int? pageSize;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public GetAll(int? page = null,
                      int? pageSize = null)
        {
            this.page = page;
            this.pageSize = pageSize;
        }

        class GetAllHandler : IRequestHandler<GetAll, IEnumerable<domain.Entities.WeatherForecast>>
        {
            private readonly ILogger<GetAllHandler> logger;

            public GetAllHandler(ILogger<GetAllHandler> logger)
            {
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<IEnumerable<domain.Entities.WeatherForecast>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                await Task.Yield();

                var rng = new Random();

                var forecasts = Enumerable.Range(1, 50).Select(index => new domain.Entities.WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });

                if (request.page.HasValue && request.pageSize.HasValue)
                {
                    var skipAmount = request.pageSize.Value * (request.page.Value - 1);
                    forecasts = forecasts
                        .Skip(skipAmount)
                        .Take(request.pageSize.Value);
                }

                return forecasts;
            }
        }
    }
}
