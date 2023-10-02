using System;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class Url
    {
        public long ID { get; set; }

        public string Address { get; set; }

        public DateTime? Found { get; set; }

        public DateTime? Collected { get; set; }

        public DateTime? IntentLocked { get; set; }

        #region Static creation methods

        public static Url Create(string address)
        {
            return new() { Address = address };
        }

        public static Url Create(string address, DateTime? found)
        {
            return new() { Address = address, Found = found };
        }

        //public static Url Create(string address, DateTime? found, DateTime? collected)
        //{
        //    return new() { Address = address, Found = found, Collected = collected };
        //}

        public static Url Create(string address, DateTime? found, DateTime? collected, DateTime? intentLocked)
        {
            return new() { Address = address, Found = found, Collected = collected, IntentLocked = intentLocked };
        }

        //public static Url Create(string address, DateTime? found, DateTime? collected, DateTime? intentLocked, int id)
        //{
        //    return new() { Address = address, Found = found, Collected = collected, IntentLocked = intentLocked, ID = id };
        //}

        #endregion
    }
}
