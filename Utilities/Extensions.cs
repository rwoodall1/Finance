using Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace Utilities {
    public static class Extensions {
        public static string Right(this string value, int length) {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        public static string Left(this string value, int maxLength) {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                    ? value
                    : value.Substring(0, maxLength)
                    );
        }

        //public static List<ApiProcessingError> ToApiProcessingErrorList(this IEnumerable<string> listOfErrors) {
        //    var returnList = new List<ApiProcessingError>();
        //    foreach (var error in listOfErrors) {
        //        if (error != "") {
        //            returnList.Add(new ApiProcessingError(error, error, "MEERR"));
        //        }
        //    }
        //    return returnList;
        //}
        public static string NormalizeName(this string text) {
            return text.Replace("\"", "");
        }

        public static string ConvertToDatabaseTimezone(this string text, string ConnectionString) {
            try {
                return ConvertToDatabaseTimezone(Convert.ToDateTime(text), ConnectionString).ToString();
            } catch {
                return null;
            }
        }

        public static DateTime? ConvertToDatabaseTimezone(this DateTime? date, string ConnectionString) {
            try {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse(date.ToString()), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            } catch {
                return null;
            }
        }

        public static DateTime ConvertToDatabaseTimezone(this DateTime date, string ConnectionString) {
            DateTime? myDate = date;
            DateTime? myReturnDate = ConvertToDatabaseTimezone(myDate, ConnectionString);
            if (myReturnDate != null) {
                return (DateTime)myReturnDate;
            } else {
                return date;
            }
        }

        public static string ConvertFromDatabaseTimezoneToUTC(this string text, string ConnectionString) {
            try {
                return ConvertFromDatabaseTimezoneToUTC(Convert.ToDateTime(text), ConnectionString).ToString();
            } catch {
                return null;
            }
        }

        public static DateTime? ConvertFromDatabaseTimezoneToUTC(this DateTime? date, string ConnectionString) {
                try {
                    return TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(date.ToString()), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                } catch {
                    return null;
                }
        }

        public static DateTime ConvertFromDatabaseTimezoneToUTC(this DateTime date, string ConnectionString) {
            DateTime? myDate = date;
            DateTime? myReturnDate = ConvertFromDatabaseTimezoneToUTC(myDate, ConnectionString);
            if (myReturnDate != null) {
                return (DateTime)myReturnDate;
            } else {
                return date;
            }
        }

        public static string ConvertFromISOToDatetime(this string text) {
            try {
                return ConvertFromISOToDatetime(Convert.ToDateTime(text)).ToString();
            } catch {
                return null;
            }
        }

        public static DateTime? ConvertFromISOToDatetime(this DateTime? date) {
            try {
                return DateTime.Parse(date.ToString(), null, DateTimeStyles.RoundtripKind);
            } catch {
                return null;
            }
        }

        #region NoNull

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>System.String</returns>
        public static string NoNull(this string obj) {
            return obj != null ? obj.ToString() : String.Empty;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.Int32&gt;</returns>
        public static int? NoNull(this int? obj) {
            return obj != null ? obj : (int)0;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.Int64&gt;</returns>
        public static long? NoNull(this long? obj) {
            return obj != null ? obj : (long)0;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.Single&gt;</returns>
        public static float? NoNull(this float? obj) {
            return obj != null ? obj : (float)0;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.Double&gt;</returns>
        public static double? NoNull(this double? obj) {
            return obj != null ? obj : (double)0;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.Boolean&gt;</returns>
        public static bool? NoNull(this bool? obj) {
            return obj != null ? obj : false;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Nullable&lt;System.DateTime&gt;</returns>
        public static DateTime? NoNull(this DateTime? obj) {
            return obj != null ? obj : DateTime.Today;
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>System.Guid</returns>
        public static Guid NoNull(this Guid obj) {
            return obj != null ? obj : new Guid();
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <typeparam name="T">The type of data in the collection.</typeparam>
        /// <param name="obj">The object</param>
        /// <returns>System.Collections.ObjectModel.ObservableCollection&lt;T&gt;</returns>
        public static ObservableCollection<T> NoNull<T>(this ObservableCollection<T> obj) {
            return obj != null ? obj : new ObservableCollection<T>();
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <typeparam name="T">The type of data in the enumerable.</typeparam>
        /// <param name="obj">The object</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;T&gt;</returns>
        public static IEnumerable<T> NoNull<T>(this IEnumerable<T> obj) {
            return obj != null ? obj : new ObservableCollection<T>();
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <typeparam name="T">The type of data in the list.</typeparam>
        /// <param name="obj">The object</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;</returns>
        public static List<T> NoNull<T>(this List<T> obj) {
            return obj != null ? obj : new List<T>();
        }

        /// <summary>
        /// Returns a non-null instance of the data type.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>System.Object</returns>
        public static object NoNull(this object obj) {
            return obj != null ? obj : new object();
        }

        #endregion

        #region String Methods

        /// <summary>
        /// Phone Format Types
        /// </summary>
        public enum PhoneFormatType {
            DEFAULT = 0
            , DOTS = 1
            , DASHES = 2
            , SPACES = 3
            , SPACEDASH = 4
            , E164 = 5
        }

        /// <summary>
        /// Formats phone string; supporting some international formats and extensions.
        /// </summary>
        /// <example>"5555551212x123".PhoneFormat() => "(555) 555-1212 x123&lt;br&gt;
        /// "15555551212".PhoneFormat() => "1 (555) 555-1212"&lt;br&gt;
        /// "5551212".PhoneFormat() => "555-1212"&lt;br&gt;
        /// "5555551212".PhoneFormat("e.164") =></example>
        /// <param name="phone">The string of the phone number.</param>
        /// <param name="format">Format style (default, dots, dashes, spaces, spacedash, e.164)</param>
        /// <returns>System.String</returns>
        public static string PhoneFormat(this string phone, string format = "default") {
            PhoneFormatType type = PhoneFormatType.DEFAULT;
            switch (format.ToLower()) {
                case "dots":
                    type = PhoneFormatType.DOTS; break;
                case "dashes":
                    type = PhoneFormatType.DASHES; break;
                case "spaces":
                    type = PhoneFormatType.SPACES; break;
                case "spacedash":
                    type = PhoneFormatType.SPACEDASH; break;
                case "e.164":
                    type = PhoneFormatType.E164; break;
                default:
                    type = PhoneFormatType.DEFAULT; break;
            }
            return PhoneFormat(phone, type);
        }

        /// <summary>
        /// Formats phone string; supporting some international formats and extensions.
        /// </summary>
        /// <example>"5555551212x123".PhoneFormat() => "(555) 555-1212 x123&lt;br&gt;
        /// "15555551212".PhoneFormat() => "1 (555) 555-1212"&lt;br&gt;
        /// "5551212".PhoneFormat() => "555-1212"&lt;br&gt;
        /// "5555551212".PhoneFormat(PhoneFormatType.E164) =></example>
        /// <param name="phone">The string of the phone number.</param>
        /// <param name="format">Format style enum (DEFAULT, DOTS, DASHES, SPACES, SPACEDASH, E164)</param>
        /// <returns>System.String</returns>
        public static string PhoneFormat(this string phone, PhoneFormatType format = PhoneFormatType.DEFAULT) {
            #region Format Definitions
            string[,] formats = new string[13, 6];
            /*               DEFAULT                                 DOTS                                DASHES                              SPACES                              SPACEDASH                           E.164          */
            formats[7, 0] = "###-####"; formats[7, 1] = "###'.'####"; formats[7, 3] = "### ####";
            formats[10, 0] = "(###) ###-####"; formats[10, 1] = "###'.'###'.'####"; formats[10, 2] = "###-###-####"; formats[10, 3] = "### ### ####"; formats[10, 4] = "### ###-####"; formats[10, 5] = "+1##########";
            formats[11, 0] = "# (###) ###-####"; formats[11, 1] = "#'.'###'.'###'.'####"; formats[11, 2] = "#-###-###-####"; formats[11, 3] = "# ### ### ####"; formats[11, 4] = "# ### ###-####"; formats[11, 5] = "+###########";
            formats[12, 0] = "+## ## #### ####";
            int style = (int)format;
            #endregion
            string formatted = ""; string _ext; long _xt;
            string _pre = Regex.Replace(phone.NoNull().ToLower(), "[^0-9x]", "");
            if (_pre.Length > 0) {
                #region Parse Number
                string _num = _pre.Contains("x") ? _pre.Substring(0, _pre.IndexOf("x")) : _pre;
                try { _ext = _pre.Contains("x") ? _pre.Substring(_pre.IndexOf("x") + 1, _pre.Length - _pre.IndexOf("x") - 1) : "0"; } catch { _ext = "0"; }
                long _ph = Convert.ToInt64(_num); int px = _ph.ToString().Length;
                try { _xt = Convert.ToInt64(_ext); } catch { _xt = 0; }
                #endregion

                #region Format the number

                if (formats[px, 0] != null) {
                    if (formats[px, style] == null) { style = 0; } // format style was not found for the phone length; use default
                    formatted = _ph.ToString(formats[px, style]);
                } else {
                    formatted = _ph.ToString(); // no format style; output number unformatted
                }

                // Append extension (if applicable)
                if (_xt > 0) { formatted = formatted + " x" + _xt.ToString(); }

                #endregion
            }

            return formatted;
        }

        /// <summary>
        /// Unformats phone string. Removes all non-numeric characters and removes extension (if applicable).
        /// </summary>
        /// <param name="phone">The string of the phone number.</param>
        /// <returns>System.String</returns>
        public static string PhoneUnFormat(this string phone) {
            string _pre = Regex.Replace(phone.NoNull(), "[^0-9x]", "");
            return _pre.Contains("x") ? _pre.Substring(0, _pre.IndexOf("x")) : _pre;
        }

        /// <summary>
        /// Strips HTML tags from a given string.
        /// </summary>
        /// <param name="str">The string of the HTML code.</param>
        /// <returns>System.String</returns>
        public static string StripHtml(this string str) {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return Regex.Replace(str, @"<[^>]*>", string.Empty);
        }

        /// <summary>
        /// Removes last character from input string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>System.String</returns>
        public static string RemoveLastCharacter(this string str) {
            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// Removes last X characters from input string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="number">Number of characters to remove</param>
        /// <returns>System.String</returns>
        public static string RemoveLast(this string str, int number) {
            return str.Substring(0, str.Length - number);
        }

        /// <summary>
        /// Removes first character from input string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>System.String</returns>
        public static string RemoveFirstCharacter(this string str) {
            return str.Substring(1);
        }

        /// <summary>
        /// Removes first X characters from input string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="number">Number of characters to remove</param>
        /// <returns>System.String</returns>
        public static string RemoveFirst(this string str, int number) {
            return str.Substring(number);
        }

        /// <summary>
        /// Reverses entire string content
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>System.String</returns>
        public static string Reverse(this string str) {
            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Removes any non-digit characters from string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>System.String</returns>
        public static string OnlyDigits(this string str) {
            return new string(str?.Where(c => char.IsDigit(c)).ToArray());
        }

        /// <summary>
        /// Removes diacritics from string (i.e. á, ñ, ē, ÿ, etc.)
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>System.String</returns>
        public static string RemoveDiacritics(this string str) {
            string stFormD = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++) {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark) {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Regular expression matching of specified pattern.
        /// </summary>
        /// <param name="str">The string to match against.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <returns>System.Boolean</returns>
        public static bool Matches(this string str, string pattern) {
            return Regex.IsMatch(str, pattern);
        }

        #endregion

        #region Date Methods

        /// <summary>
        /// Returns whether the base date falls between the range dates.
        /// </summary>
        /// <param name="date">The base date</param>
        /// <param name="rangeBeg">Beginning range date</param>
        /// <param name="rangeEnd">Ending range date</param>
        /// <returns>System.Boolean</returns>
        public static bool Between(this DateTime date, DateTime rangeBeg, DateTime rangeEnd) {
            return date.Ticks >= rangeBeg.Ticks && date.Ticks <= rangeEnd.Ticks;
        }

        /// <summary>
        /// Returns the age (in years) of the specified date.
        /// </summary>
        /// <param name="date">The base date</param>
        /// <returns>System.Int32</returns>
        public static int CalculateAge(this DateTime date) {
            var age = DateTime.Now.Year - date.Year;
            if (DateTime.Now < date.AddYears(age))
                age--;
            return age;
        }

        /// <summary>
        /// Converts a date's age into a friendly readable format. (i.e. "20 seconds ago", "5 hours ago", "1 year ago", etc.)
        /// </summary>
        /// <param name="date">The base date</param>
        /// <returns>System.String</returns>
        public static string ToReadableTime(this DateTime date) {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = ts.TotalSeconds;
            if (delta < 60) {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120) {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

        /// <summary>
        /// Returns whether the specified date is a working day (M-F)
        /// </summary>
        /// <param name="date">The base date</param>
        /// <returns>System.Boolean</returns>
        public static bool WorkingDay(this DateTime date) {
            return !date.IsWeekend();
        }

        /// <summary>
        /// Returns whether the specified date is on a weekend (Sat/Sun)
        /// </summary>
        /// <param name="date">The base date</param>
        /// <returns>System.Boolean</returns>
        public static bool IsWeekend(this DateTime date) {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Returns the next workday following the specified date.
        /// </summary>
        /// <param name="date">The base date</param>
        /// <returns>System.Boolean</returns>
        public static DateTime NextWorkday(this DateTime date) {
            var nextDay = date;
            while (!nextDay.WorkingDay()) {
                nextDay = nextDay.AddDays(1);
            }
            return nextDay;
        }

        /// <summary>
        /// Returns the next day following the specified date which is on the specified day of week.
        /// </summary>
        /// <param name="date">The base date</param>
        /// <param name="dayOfWeek">Day of Week to target</param>
        /// <returns>System.Boolean</returns>
        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek) {
            int offsetDays = dayOfWeek - date.DayOfWeek;
            if (offsetDays <= 0) {
                offsetDays += 7;
            }
            DateTime result = date.AddDays(offsetDays);
            return result;
        }

        /// <summary>
        /// Determines if a string specified is a valid date.
        /// </summary>
        /// <param name="value">The string of the date.</param>
        /// <returns>System.Boolean</returns>
        public static bool ValidDate(string value) {
            bool success = true;
            try { DateTime.Parse(value); } catch { success = false; }
            return success;
        }

        #endregion

        #region Numeric Methods

        /// <summary>
        /// Returns a formatted display of a file size. (support up to exobytes)
        /// </summary>
        /// <example>((long)1024).ToFileSize() => "1 KB"&lt;br&gt;((long)1048576).ToFileSize() => "1 MB"</example>
        /// <param name="size">The file size in bytes</param>
        /// <returns>System.String</returns>
        public static string ToFileSize(this Int64 size) {
            if (size < 1024) { return (size).ToString("F0") + " bytes"; }
            if (size < Math.Pow(1024, 2)) { return (size / 1024).ToString("F0") + "KB"; }
            if (size < Math.Pow(1024, 3)) { return (size / Math.Pow(1024, 2)).ToString("F0") + "MB"; }
            if (size < Math.Pow(1024, 4)) { return (size / Math.Pow(1024, 3)).ToString("F0") + "GB"; }
            if (size < Math.Pow(1024, 5)) { return (size / Math.Pow(1024, 4)).ToString("F0") + "TB"; }
            if (size < Math.Pow(1024, 6)) { return (size / Math.Pow(1024, 5)).ToString("F0") + "PB"; }
            return (size / Math.Pow(1024, 6)).ToString("F0") + "EB";
        }

        #endregion

        #region Coordinate/Distance Methods

        /// <summary>
        /// Returns a value of conversion for meters to miles.
        /// </summary>
        /// <param name="meters">The distance in meters</param>
        /// <returns>System.Double</returns>
        public static double ConvertMetersToMiles(this double meters) {
            return (meters / 1609.344);
        }

        /// <summary>
        /// Returns a value of conversion for miles to meters.
        /// </summary>
        /// <param name="miles">The distance in miles</param>
        /// <returns>System.Double</returns>
        public static double ConvertMilesToMeters(this double miles) {
            return (miles * 1609.344);
        }

        /// <summary>
        /// Returns a value of conversion for kilometers to miles.
        /// </summary>
        /// <param name="km">The distance in kilometers</param>
        /// <returns>System.Double</returns>
        public static double ConvertKilometersToMiles(this double km) {
            return (km / 1.609344);
        }

        /// <summary>
        /// Returns a value of conversion for miles to kilometers.
        /// </summary>
        /// <param name="miles">The distance in miles</param>
        /// <returns>System.Double</returns>
        public static double ConvertMilesToKilometers(this double miles) {
            return (miles * 1.609344);
        }

        #endregion

        #region List Methods

        /// <summary>
        /// Prepends to a list with the specified item/object.
        /// </summary>
        /// <typeparam name="T">Type of data in the list.</typeparam>
        /// <param name="list">The list to be prepended.</param>
        /// <param name="item">The item to prepend to the list.</param>
        /// <returns></returns>
        public static List<T> Prepend<T>(this List<T> list, T item) {
            list.Insert(0, item);
            return list;
        }

        #endregion
    }
}
