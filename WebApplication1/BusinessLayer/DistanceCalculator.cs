using System;
using System.Collections.Generic;

namespace WebApplication1.BusinessLayer
{
        //#3 This is New Implemented Section.
    public static class DistanceCalculator
    {

        private static readonly Dictionary<string, (decimal Lat, decimal Lon)> LocationCoordinates
            = new Dictionary<string, (decimal, decimal)>
        {
            { "1", (7.2906m, 80.6337m) },  // Colombo
            { "2", (7.2300m, 80.0000m) },  // Gampaha
            { "3", (6.9833m, 79.8833m) },  // Negombo
            { "4", (6.7000m, 80.0000m) },  // Kalutara
            { "5", (7.1500m, 80.1500m) }   // Veyangoda

        };

        private static readonly (decimal Lat, decimal Lon) WorkplaceCoordinates
            = (7.2906m, 80.6337m);

        public static decimal GetDistanceToWorkplace(string locationId)
        {
            // Guard clause: empty or null location ID
            if (string.IsNullOrWhiteSpace(locationId))
                return 0;

            // Look up coordinates from dictionary
            if (!LocationCoordinates.TryGetValue(locationId, out var employeeCoords))
                return 0;

            // Calculate distance using Haversine formula
            return CalculateDistance(
                employeeCoords.Lat,
                employeeCoords.Lon,
                WorkplaceCoordinates.Lat,
                WorkplaceCoordinates.Lon
            );
        }


        public static (decimal Lat, decimal Lon) GetLocationCoordinates(string locationId)
        {
            if (string.IsNullOrWhiteSpace(locationId))
                return (0, 0);

            return LocationCoordinates.TryGetValue(locationId, out var coords)
                ? coords
                : (0, 0);
        }


        public static (decimal Lat, decimal Lon) GetWorkplaceCoordinates()
            => WorkplaceCoordinates;

        public static string GetWorkplaceName()
            => "Colombo Head Office";

        public static string FormatDistance(decimal distanceKm)
        {
            if (distanceKm < 1)
                return $"{Math.Round(distanceKm * 1000, 0)} meters";
            return $"{Math.Round(distanceKm, 2)} km";
        }


        private static decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const decimal EarthRadiusKm = 6371m;

            // Convert degrees to radians
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var lat1Rad = ToRadians(lat1);
            var lat2Rad = ToRadians(lat2);

            // Haversine formula
            var a = (decimal)Math.Pow((double)Math.Sin((double)dLat / 2), 2) +
                    (decimal)Math.Cos((double)lat1Rad) *
                    (decimal)Math.Cos((double)lat2Rad) *
                    (decimal)Math.Pow((double)Math.Sin((double)dLon / 2), 2);

            var c = 2 * (decimal)Math.Atan2(
                (double)Math.Sqrt((double)a),
                (double)Math.Sqrt(1 - (double)a)
            );

            return EarthRadiusKm * c;
        }


        private static decimal ToRadians(decimal degrees)
            => degrees * (decimal)Math.PI / 180;
    }
}