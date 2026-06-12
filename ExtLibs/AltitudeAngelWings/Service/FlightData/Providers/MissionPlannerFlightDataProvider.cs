using System;
using AltitudeAngelWings.Model;

namespace AltitudeAngelWings.Service.FlightData.Providers
{
    public class XagSurveillanceGCSFlightDataProvider : IFlightDataProvider
    {
        private const int GeographicPrecision = 7;
        private const int AltitudePrecision = 2;

        public XagSurveillanceGCSFlightDataProvider(IXagSurveillanceGCSState XagSurveillanceGCSState)
        {
            _XagSurveillanceGCSState = XagSurveillanceGCSState;
        }

        public Model.FlightData GetCurrentFlightData()
        {
            return new Model.FlightData
            {
                Armed = _XagSurveillanceGCSState.IsArmed,
                CurrentPosition = new FlightDataPosition
                {
                    Longitude = Math.Round(_XagSurveillanceGCSState.Longitude, GeographicPrecision, MidpointRounding.AwayFromZero),
                    Latitude = Math.Round(_XagSurveillanceGCSState.Latitude, GeographicPrecision, MidpointRounding.AwayFromZero),
                    Altitude = Math.Round(_XagSurveillanceGCSState.Altitude, AltitudePrecision, MidpointRounding.AwayFromZero),
                    Course = _XagSurveillanceGCSState.GroundCourse,
                    Speed = _XagSurveillanceGCSState.GroundSpeed,
                    VerticalSpeed = _XagSurveillanceGCSState.VerticalSpeed
                }
            };
        }

        private readonly IXagSurveillanceGCSState _XagSurveillanceGCSState;
    }
}
