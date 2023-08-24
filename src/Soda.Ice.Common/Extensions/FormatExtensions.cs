namespace Soda.Ice.Common.Extensions
{
    public static class FormatExtensions
    {
        public static string ToFormatDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}