using System;

namespace dotnet.playground.domain.Entities
{
    /// <summary>
    /// Weather Forecast Entity
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Forecast date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Temperature in Celsius degrees
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in fahrenheit degreees
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Syntehtic description
        /// </summary>
        public string Summary { get; set; }
    }
}
