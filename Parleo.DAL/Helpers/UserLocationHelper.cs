using System;

namespace Parleo.DAL.Helpers
{
    public static class UserLocationHelper
    {
        private const double KILOMETERS_COEFFICIENT = 1.609344;
        private const double DEGREES_MULTIPLIER = 60 * 1.1515;

        public static double GetDistanceBetween(double lon1, double lat1, double lon2, double lat2)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double distance = Math.Sin(DegreesToRadians(lat1)) * Math.Sin(DegreesToRadians(lat2)) +
                              Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                              Math.Cos(DegreesToRadians(theta));

                distance = Math.Acos(distance);
                distance = RadiansToDegrees(distance);
                distance = distance * DEGREES_MULTIPLIER;
                distance = distance * KILOMETERS_COEFFICIENT;

                return distance;
            }
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI / 180);
        }

        private static double RadiansToDegrees(double radians)
        {
            return (radians / Math.PI * 180);
        }
    }
}
