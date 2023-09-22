using System;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public abstract class DataTransferObject
    {
        public static long ToUnixDate(DateTime? possibleValue, DateTime defaultValue)
        {
            if (possibleValue.HasValue)
            {
                return ((DateTimeOffset)possibleValue).ToUnixTimeSeconds();
            }

            return ((DateTimeOffset)defaultValue).ToUnixTimeSeconds();
        }

        public static long ToUnixDate(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static DateTime ToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dateTime;
        }
    }
}
