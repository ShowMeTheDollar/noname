using System.ComponentModel.DataAnnotations;
using ApplicationCore.Enums;


namespace OpenRoom.Web.Helpers
{
    public static class DayOfWeekEnumHelper
    {
        public static string GetDisplayValue(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "週一";
                case DayOfWeek.Tuesday:
                    return "週二";
                case DayOfWeek.Wednesday:
                    return "週三";
                case DayOfWeek.Thursday:
                    return "週四";
                case DayOfWeek.Friday:
                    return "週五";
                case DayOfWeek.Saturday:
                    return "週六";
                case DayOfWeek.Sunday:
                    return "週日";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
