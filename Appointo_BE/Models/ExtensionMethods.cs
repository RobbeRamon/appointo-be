﻿using System;

namespace Appointo_BE.Models
{
    public static class ExtensionMethods
    {
        public static void AddTime(this Time time1, Time time2)
        {
            DateTime dateTime = new DateTime(0, 0, 0, time1.Hour, time1.Minute, time2.Second);
            dateTime.AddHours(time2.Hour);
            dateTime.AddMinutes(time2.Minute);
            dateTime.AddSeconds(time2.Second);

            time1.Hour = dateTime.Hour;
            time1.Minute = dateTime.Minute;
            time1.Second = dateTime.Second;
        }
    }
}
